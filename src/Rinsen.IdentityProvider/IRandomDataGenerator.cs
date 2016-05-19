namespace Rinsen.IdentityProvider
{
    public interface IRandomDataGenerator
    {
        byte[] GetRandomByteArray(int length);
        string GetRandomString(int length);
    }
}