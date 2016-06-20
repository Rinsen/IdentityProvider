using System.Security.Cryptography;

namespace Rinsen.IdentityProvider.Core
{
    public class RandomDataGenerator : IRandomDataGenerator
    {
        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        static readonly char[] AvailableCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public string GetRandomString(int length)
        {
            char[] identifier = new char[length];
            byte[] randomData = GetRandomByteArray(length);

            for (int idx = 0; idx < identifier.Length; idx++)
            {
                int pos = randomData[idx] % AvailableCharacters.Length;
                identifier[idx] = AvailableCharacters[pos];
            }

            return new string(identifier);
        }

        public byte[] GetRandomByteArray(int length)
        {
            byte[] randomData = new byte[length];

            CryptoRandom.GetBytes(randomData);
            
            return randomData;
        }

    }
}
