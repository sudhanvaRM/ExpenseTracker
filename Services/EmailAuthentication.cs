using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Server.Models.Entities;

namespace Server.Services
{
    public class EmailAuthentication
    {  
 
      private readonly ApplicationDbContext _context;
        
    public EmailAuthentication(ApplicationDbContext context)
    {
        _context = context;
    }
        // Method to validate the email format
        public bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // Regex pattern for email validation
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }
        
        public bool IsValidUsername(String username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return false;
            }

            //Regex pattern for username validation
            string usernamePattern = "^[A-Za-z][A-Za-z0-9_]{7,29}$";
            return Regex.IsMatch(username, usernamePattern);
        }
        
        //Method to Check whether user already exists in database
        public bool IsExistentUser(string email)
        {
             var user = _context.Users.FirstOrDefault(u => u.Email == email);
        
            // If the user is found, return true; otherwise, return false
            return user != null;
        }
    }
}
