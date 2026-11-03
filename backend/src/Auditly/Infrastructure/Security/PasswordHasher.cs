using System.Security.Cryptography;
using System.Text;
using Domain.Interface;

namespace Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public (byte[] hash, byte[] salt) Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, 
                salt, 
                Iterations, 
                Algorithm, 
                HashSize);
            return (hash, salt);
        }

        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('-');
            byte[] passwordHash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(passwordHash, inputHash);
        }
    }
}
