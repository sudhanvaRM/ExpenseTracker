using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
   
  public class SignupDTO
  {
     [Required(ErrorMessage = "Name is required")]
     public String Username { get; set; } = null!;

     [Required(ErrorMessage = "Email ID is required")]
    //  [EmailAddress(ErrorMessage = "Invalid email format")]
     public String EmailID { get; set; } = null!;

     [Required(ErrorMessage = "Password is required")]
     public String Password { get; set; }  = null!;
  }
}

 

