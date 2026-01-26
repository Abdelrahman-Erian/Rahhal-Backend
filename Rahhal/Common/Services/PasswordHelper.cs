
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rahhal.Common.Interfaces.PasswordServices;
using System.Security.Cryptography;

namespace Rahhal.Common.Services.PasswordServices
{
        public class PasswordHelper : IPasswordHelper
    {
            private readonly int _iterations = 10000;
            private readonly int _saltSize = 16;
            private readonly int _hashSize = 32;

            public (string salt, string hash) createPasswordHash(string password)
            {
              
                var random = RandomNumberGenerator.Create();
                var salt = new byte[_saltSize];
                random.GetBytes(salt);

             
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(_hashSize);

                var salted = Convert.ToBase64String(salt);
                var hashed = Convert.ToBase64String(hash);

                var hashedPassword = $"{_iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                return (salted, hashed);

            }

            public bool verifyPasswordHash(string password, string passwordHash, string salted)
            {

                
                var salt = Convert.FromBase64String(salted);
                var storedHash = Convert.FromBase64String(passwordHash);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);
                var computedHash = pbkdf2.GetBytes(_hashSize);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);

            }
        }
    }

