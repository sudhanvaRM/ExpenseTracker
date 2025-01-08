using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "EmailID is Required")]
        public string EmailID { get; set; } = null!;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = null!;
    }
}