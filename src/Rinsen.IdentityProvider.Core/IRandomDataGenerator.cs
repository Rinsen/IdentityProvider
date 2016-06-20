namespace Rinsen.IdentityProvider.Core
{
    public interface IRandomDataGenerator
    {
        byte[] GetRandomByteArray(int length);
        string GetRandomString(int length);
    }
}