using System.Security.Cryptography;

namespace Domain.Interface
{
    public interface IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
        
        public (byte[] hash, byte[] salt) Hash(string password);
        public bool VerifyHashedPassword(string password, string hashedPassword);
    }
}
