using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;  // Namespace where SignupDTO is defined
using Server.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly EmailAuthentication _emailAuthentication;
        private readonly PasswordHashService _passwordHash;
        private readonly JwtToken _jwt;

        // Constructor to inject services
        public SignupController(EmailAuthentication emailAuthentication, PasswordHashService passwordHash, JwtToken jwt)
        {
            _emailAuthentication = emailAuthentication;
            _passwordHash = passwordHash;
            _jwt = jwt;
        }

        // POST API for user signup
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] SignupDTO signup)
        {
            // Validate the payload
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    Message = "Invalid data", 
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
                });
            }

            if(!_emailAuthentication.IsValidUsername(signup.Username))
            {
                return BadRequest(new {Message = "Invalid Username format"});
            }

            // Validate email format
            if (!_emailAuthentication.IsValidEmailFormat(signup.EmailID))
            {
                return BadRequest(new { Message = "Invalid email format." });
            }

            // Check if the email already exists
            if (_emailAuthentication.IsExistentUser(signup.EmailID))
            {
                return Conflict(new { Message = "EmailId already exists." });
            }

            // Hash the password and create the user
            var result = await _passwordHash.CreateHashPasswordAsync(signup);

            if (result.IsSuccess)
            {
                string tokenValue = await _jwt.GenerateJwtToken(signup.EmailID);
                return Ok(new { Message = result.Message, TokenValue = tokenValue});
            }
            else
            {   
                return StatusCode(500, new { Message = "Error while registering user."});
            }
        }
    }
}
