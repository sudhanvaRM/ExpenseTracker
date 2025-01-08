using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class EditExpenseDTO
    {
        [Required(ErrorMessage ="Enter the transaction ID")]
        public int DebitID {get; set; }
        public decimal? Amount{ get; set;}
        public String? Category{ get; set;} 
         public String? Comment{ get; set;} 
    }
}