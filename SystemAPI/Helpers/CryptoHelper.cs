using Shared.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace SystemAPI.Helpers
{
    /// <summary>
    /// Helper class for cryptographic operations
    /// </summary>
    public static class CryptoHelper
    {
        /// <summary>
        /// Generate a random salt of the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateSalt( int length = 16 )
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator.Fill(salt);

            return salt;
        }

        /// <summary>
        /// Generate a random salt string of the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateSaltString( int length = 16 ) => Convert.ToBase64String(GenerateSalt(length)).Replace("/","-");

        /// <summary>
        /// Generate a random salt string of the specified length
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] HashPassword(string password, byte[] salt )
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(32);
        }

        /// <summary>
        /// Compare two byte arrays in a way that is not vulnerable to timing attacks
        /// </summary>
        /// <param name="hashA"></param>
        /// <param name="hashB"></param>
        /// <returns></returns>
        public static bool CompareBytes(byte[] hashA, byte[] hashB) => CryptographicOperations.FixedTimeEquals(hashA, hashB);

        /// <summary>
        /// Compare a password with a hashed password and salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool ComparePasswordWithHash( string password, byte[] hashedPassword, byte[] salt )
        {
            byte[] newlyHashedPassword = HashPassword(password, salt);

            return CompareBytes(newlyHashedPassword, hashedPassword);
        }
    }
}
