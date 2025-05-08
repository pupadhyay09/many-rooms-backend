using System.Security.Cryptography;

namespace ManyRoomStudio.Infrastructure.Helpers
{
    public static class SecurityHelper
    {
        /// <summary>
        /// Generates a random salt.
        /// </summary>
        /// <param name="size">Size of the salt in bytes (default is 16 bytes).</param>
        /// <returns>Base64-encoded salt string.</returns>
        public static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hashes a password with the given salt using PBKDF2.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <param name="salt">The salt to use for hashing.</param>
        /// <param name="iterations">Number of iterations (default is 10000).</param>
        /// <param name="hashSize">Size of the hash in bytes (default is 32 bytes).</param>
        /// <returns>Base64-encoded hashed password.</returns>
        public static string HashPassword(string password, string salt, int iterations = 10000, int hashSize = 32)
        {
            // Convert salt from Base64 to byte array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Generate the hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(hashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
