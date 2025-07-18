using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace asg
{
    
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = GenerateSalt();

            // Use PBKDF2 to hash the password with the salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
            {
                byte[] hash = pbkdf2.GetBytes(32); // Generate a 256-bit hash (32 bytes)

                // Combine salt and hash for storage
                byte[] hashBytes = new byte[48]; // 16 bytes for salt + 32 bytes for hash
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                // Convert to Base64 string for storage in the database
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            // Decode the Base64-encoded hash
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract salt (first 16 bytes) and stored hash (next 32 bytes)
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            byte[] storedPasswordHash = new byte[32];
            Array.Copy(hashBytes, 16, storedPasswordHash, 0, 32);

            // Hash the input password using the same salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                // Compare the newly computed hash with the stored hash
                for (int i = 0; i < 32; i++)
                {
                    if (hash[i] != storedPasswordHash[i])
                        return false;
                }
            }

            return true;
        }

        private static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt); // Fill the array with cryptographically secure random bytes
                return salt;
            }
        }
    }

}