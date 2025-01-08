using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Entities
{
    [Table("splits")] // Maps the class to the "splits" table
    public class Split
    {
        [Key]
        [Column("split_id")]
        public int SplitId { get; set; } // Primary Key

        [Column("transaction_id")]
        public int TransactionId { get; set; } // Foreign Key to "transactions"

        [Column("user_id")]
        public int UserId { get; set; } // Foreign Key to "users"

        [Column("total_amount")]
        public decimal? TotalAmount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } 

        // Navigation Properties
        [ForeignKey("TransactionId")]
        
        public required Expense Expense { get; set; } // Related transaction

        [ForeignKey("UserId")]
        public required User User { get; set; } // Related user
    }
}
