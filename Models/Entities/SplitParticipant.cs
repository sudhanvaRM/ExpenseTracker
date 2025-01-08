
namespace Server.Models.Entities
{
   public class SplitParticipant
   {
    public int SplitId { get; set; }
    public int ParticipantId { get; set; }
    public decimal Amount { get; set; }
    public bool PaidStatus { get; set; } = false;

    public int TransactionID {get ; set;}

    // Navigation properties
    public virtual required Split Split { get; set; }  // Link to the Split table
    public virtual required User Participant { get; set; }  // Link to the User table

     public Expense Expense { get; set; } = null!;
   }
}



