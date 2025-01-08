using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class DeleteExpenseDTO
    {
         [Required(ErrorMessage = "Mention which expense to delete")]
         public int DebitID  { get; set; }
    }
}