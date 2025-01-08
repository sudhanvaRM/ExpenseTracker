using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services; 
using System;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        // Constructor injection for ApplicationDbContext and LoginAuthentication services
        private readonly EmailAuthentication  _emailAuthentication;
        private readonly PasswordHashService _passwordHash;
        // private readonly IConfiguration _configuration;
        private readonly JwtToken _jwt;

        public LoginController(EmailAuthentication emailAuthentication, PasswordHashService passwordHash,IConfiguration configuration,JwtToken jwt)
        {
            _emailAuthentication = emailAuthentication;
            _passwordHash = passwordHash;
            _jwt = jwt;
        }

        // This endpoint handles login and validates the email format
        [HttpPost]
        public async Task<IActionResult>  Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                // Return a BadRequest with the validation errors
                return BadRequest(new { Message = "Invalid data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
           
            if (!_emailAuthentication.IsValidEmailFormat(login.EmailID))
            {
                return BadRequest(new { Message = "Invalid email format." });
            }

            var result = await _passwordHash.VerifyPassword(login.EmailID,login.Password);

            
            if (result)
            {   
              string tokenValue = await _jwt.GenerateJwtToken(login.EmailID);
              return Ok( new {token = tokenValue,  Message = "Login Successful"});
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
        }
        
    }
}
