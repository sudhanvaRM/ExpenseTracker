using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Server.Models;


namespace Server.Services
{
    public class JwtToken
    {
     
       private readonly PasswordHashService _passwordHash;
       private readonly IConfiguration _configuration;
       public JwtToken(PasswordHashService passwordHash, IConfiguration configuration)
       {
         _passwordHash = passwordHash;
         _configuration = configuration;
       }

       public async Task<String> GenerateJwtToken(String emailID)
       {
         int userId = await _passwordHash.FetchUserId(emailID);
         var claims = new[]
         {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", userId.ToString())
         };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

         var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
         var token = new JwtSecurityToken(
             issuer: _configuration["Jwt:Issuer"],
             audience: _configuration["Jwt:Audience"],
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(60),
             signingCredentials: signin
         );

         string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
         return tokenValue;
       }
       
       public int FetchUserIdfromJWT(String jwtToken)
       {
           var handler = new JwtSecurityTokenHandler();
           var token = handler.ReadJwtToken(jwtToken);
           var userIdClaim = token.Claims.First(c => c.Type == "userId").Value;
           int userId = int.Parse(userIdClaim);
           return userId;
       }
    }
}