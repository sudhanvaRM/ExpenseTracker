using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ExpenseDTO
    {
        [Required(ErrorMessage ="Enter the amount")]
        public int Amount{ get; set;} = 0!;

        [Required(ErrorMessage ="Invalid Category")]
        public String Category{ get; set;} = null!;

         public String? Comment{ get; set;}  //Nullable Character
    }
}