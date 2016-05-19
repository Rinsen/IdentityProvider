using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Rinsen.IdentityProvider
{
    public class PasswordHashGenerator
    {
        internal byte[] GetPasswordHash(byte[] passwordSalt, string userPassword, int iterationCount, int numBytesRequested)
        {
            return KeyDerivation.Pbkdf2(userPassword, passwordSalt, KeyDerivationPrf.HMACSHA256, iterationCount, numBytesRequested);
        }
    }
}
