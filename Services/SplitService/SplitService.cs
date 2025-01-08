using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Server.Models.Entities;


namespace Server.Services
{
    public class SplitService
    {
        private readonly ApplicationDbContext _context;

        public SplitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateTransactionID(AddSplitDTO split)
        {
            // Check if the transaction exists in the Expenses table
            var transaction = await _context.Expenses
                .FirstOrDefaultAsync(e => e.DebitId == split.TransactionID);

            return transaction != null;
        }

        public async Task<bool> CreateSplit(AddSplitDTO split, int userID)
        {
                // Ensure the Expense exists for the given transaction ID
                var expense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.DebitId == split.TransactionID && e.UserId == userID);

                if (expense == null)
                {
                    return false; // If no such expense exists, return false
                }

                // Ensure the User exists for the given user ID
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userID);

                if (user == null)
                {
                    return false; // If no such user exists, return false
                }

                decimal TotalAmount = await this.FetchAmountAsync(split.TransactionID);
                // Create a new Split object with provided data
                var newSplit = new Split
                {
                    TransactionId = split.TransactionID,
                    UserId = userID,
                    Expense = expense, // Associate the Expense with Split
                    User = user,       // Associate the User with Split
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = TotalAmount
                };

                // Add the Split to the DbContext before accessing its SplitId
                _context.Splits.Add(newSplit);
                await _context.SaveChangesAsync(); // Save to generate SplitId

                // Now handle SplitParticipants
                foreach (var participant in split.Participants)
                {
                    var participantUser = await _context.Users.FindAsync(participant.ParticipantId);

                    if (participantUser == null)
                    {
                        return false; // Participant not found
                    }

                    var newSplitParticipant = new SplitParticipant
                    {   TransactionID = split.TransactionID,
                        SplitId = newSplit.SplitId, // Set the SplitId after saving newSplit
                        Split = newSplit,            // Set the Split navigation property
                        ParticipantId = participant.ParticipantId,

                        Participant = participantUser, // Set the Participant navigation property
                        Amount = participant.Amount     // Amount the participant owes
                    };

                    _context.SplitParticipants.Add(newSplitParticipant);
                }

                // Start a transaction to ensure atomicity of the operations
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    // Save changes for the SplitParticipants
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                    {
                        // Commit the transaction if successful
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        // Rollback the transaction if failed
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
        }

        public async Task<decimal> FetchAmountAsync(int transactionID)
        {
            var amount = await _context.Expenses
                .Where(e => e.DebitId == transactionID) // Filter based on DebitId
                .Select(e => e.Amount) // Select the Amount property
                .FirstOrDefaultAsync(); // Get the first matching result or default

            return amount;
        }

        public async Task<bool> ValidateSplitID(int splitID, int userID)
        {

            var exists = await _context.SplitParticipants
                            .AnyAsync(s => s.SplitId == splitID && s.ParticipantId == userID); //AnyAsync: Checks if any record in database matches the given condition

            return exists;
        }

       public async Task<bool> PaySplit(int splitID, int userID)
       {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Step 1: Retrieve the participant entry
                    var participant = await _context.SplitParticipants
                        .FirstOrDefaultAsync(s => s.SplitId == splitID && s.ParticipantId == userID);

                    if (participant == null)
                    {
                        return false; // No matching participant found
                    }

                    // Step 2: Update PaidStatus to true
                    participant.PaidStatus = true;
                    await _context.SaveChangesAsync();

                    // Step 3: Retrieve transaction details
                    var transactionId = participant.TransactionID;
                    var amount = participant.Amount;

                    var transactionEntity = await _context.Expenses
                        .FirstOrDefaultAsync(t => t.DebitId == transactionId);

                    if (transactionEntity == null)
                    {
                        return false; // No matching transaction found
                    }

                    // Step 4: Deduct the amount from the total transaction
                    transactionEntity.Amount -= amount;
                    await _context.SaveChangesAsync();

                    // Step 5: Copy the transaction details (comment, category)
                    var comment = transactionEntity.Comment;
                    var category = transactionEntity.Category;

                    // Step 6: Create a new transaction in the Expense table
                    var newTransaction = new Expense
                    {
                        UserId = userID,
                        Comment = comment,
                        Category = category,
                        Amount = amount // Paid amount
                    };

                    _context.Expenses.Add(newTransaction);
                    await _context.SaveChangesAsync();

                    // Step 7: Commit the transaction
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    // Rollback in case of an error
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

       public async Task<List<PendingSplitDTO>> FetchPendingSplits(int userID)
       {
            var unpaidSplits = await _context.SplitParticipants
                .Where(sp => !sp.PaidStatus && sp.ParticipantId == userID) // Filter unpaid splits for the user
                .Select(sp => new PendingSplitDTO
                    {     
                        SplitID = sp.SplitId,
                        TransactionId = sp.TransactionID,
                        Amount = sp.Amount,
                        Category = sp.Expense.Category,  
                        Comment = sp.Expense.Comment    
                    })
                .ToListAsync();

            return unpaidSplits;
        }
     }
 }

