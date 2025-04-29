using Shared.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace SystemAPI.Helpers
{
    public static class CryptoHelper
    {
        public static byte[] GenerateSalt( int length = 16 )
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator.Fill(salt);

            return salt;
        }

        public static string GenerateSaltString( int length = 16 ) => Convert.ToBase64String(GenerateSalt(length)).Replace("/","-");

        public static byte[] HashPassword(string password, byte[] salt )
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(32);
        }

        public static bool CompareBytes(byte[] hashA, byte[] hashB) => CryptographicOperations.FixedTimeEquals(hashA, hashB);

        public static bool ComparePasswordWithHash( string password, byte[] hashedPassword, byte[] salt )
        {
            byte[] newlyHashedPassword = HashPassword(password, salt);

            return CompareBytes(newlyHashedPassword, hashedPassword);
        }
    }
}
