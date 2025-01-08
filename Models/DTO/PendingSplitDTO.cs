using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
   public class PendingSplitDTO
    {
        public int SplitID {get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public required string Category { get; set; }
        public string ? Comment { get; set; } 
    }
}
    
