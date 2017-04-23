using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Service.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string originalPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalPassword));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
