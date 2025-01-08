
using Server.Models;
using Server.Models.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace Server.Services
{
    public class PasswordHashService
    {
        private readonly ApplicationDbContext _context;

        public PasswordHashService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to create hashed password and store user
        public async Task<(bool IsSuccess, string Message)> CreateHashPasswordAsync(SignupDTO signup)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signup.Password);

            var newUser = new User
            {
                Username = signup.Username,
                Email = signup.EmailID,
                Password = hashedPassword,
            };

            // Save the user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return (true, "User registered successfully.");
        }

         public async Task<bool> VerifyPassword(String email, String password)
        {
              
              var storedhash = await _context.Users
                .Where(u => u.Email == email) // filter by email
                .Select(u => u.Password) // only select the password field
                .FirstOrDefaultAsync(); // get the first matching result
             
             return BCrypt.Net.BCrypt.Verify(password, storedhash);
        }

        public async Task<int> FetchUserId(String email){
            var userId = await _context.Users
               .Where(u => u.Email == email)
               .Select(u => u.UserId)
               .FirstOrDefaultAsync();

            return userId;
        }
        
    }
}
