using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Server.Services
{  
    public class ExpenseServices
    {  
        private readonly ApplicationDbContext _context;
        private readonly List<string> categoryList = new List<string>
        {
            "Food", "Clothes", "Home", "Transportation", "Bills", "Entertainment"
        };

        public  ExpenseServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> FetchExpense(int userId){
                  
               var expenses = await _context.Expenses
                                 .Where(exp => exp.UserId == userId)
                                 .ToListAsync();  
               return expenses;
        }

        public async Task<bool> DeleteExpenseAsync(int debit_id, int userId)
        {
            try
            {
                var expense = await _context.Expenses
                                            .FirstOrDefaultAsync(e => e.DebitId == debit_id && e.UserId == userId);

                if (expense != null)
                {
                    _context.Expenses.Remove(expense);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false; 
            }

            catch (Exception ex)
            {
                // Log the exception (you can use a logger)
                throw new Exception("An error occurred while deleting the expense.", ex);
            }
        }

    
        public async  Task<TopCategoryExpenseDTO> FetchTopCategory(int userId)
        {
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;
            var currentDay = currentDate.Day;

           // Year
            var yearExpense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Timestamp.Year == currentYear)
                .GroupBy(e => e.Category)
                .Select(group => new { Category = group.Key, TotalAmount = group.Sum(e => e.Amount) })
                .OrderByDescending(g => g.TotalAmount)
                .FirstOrDefaultAsync();
 
           // Month
            var monthExpense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Timestamp.Year == currentYear && e.Timestamp.Month == currentMonth)
                .GroupBy(e => e.Category)
                .Select(group => new { Category = group.Key, TotalAmount = group.Sum(e => e.Amount) })
                .OrderByDescending(g => g.TotalAmount)
                .FirstOrDefaultAsync();

           // Day
            var dayExpense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Timestamp.Year == currentYear && e.Timestamp.Month == currentMonth && e.Timestamp.Day == currentDay)
                .GroupBy(e => e.Category)
                .Select(group => new { Category = group.Key, TotalAmount = group.Sum(e => e.Amount) })
                .OrderByDescending(g => g.TotalAmount)
                .FirstOrDefaultAsync();

           // Return consolidated DTO
            return new TopCategoryExpenseDTO
            {
                YearCategory = yearExpense?.Category ?? "No data",
                YearAmount = yearExpense?.TotalAmount ?? 0,

                MonthCategory = monthExpense?.Category ?? "No data",
                MonthAmount = monthExpense?.TotalAmount ?? 0,

                DayCategory = dayExpense?.Category ?? "No data",
                DayAmount = dayExpense?.TotalAmount ?? 0
            };
        }

        public async Task<bool> UpdateExpenseAsync(int debitId, decimal? newAmount, string? newCategory, string? newComment)
        {
            // Fetch the expense record to be updated
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.DebitId == debitId);

            if (expense == null)
            {
                // Record not found
                return false;
            }

            // Update fields based on user-provided data
            if (newAmount.HasValue)
            {
                expense.Amount = newAmount.Value;
            }

            if (!string.IsNullOrEmpty(newCategory))
            {  
                if(!categoryList.Contains(newCategory, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                expense.Category = newCategory;
            }

            if (!string.IsNullOrEmpty(newComment))
            {
                expense.Comment = newComment;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true; 
        }


    }
}