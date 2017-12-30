namespace Rinsen.IdentityProvider.ExternalApplications
{
    public class IdentityResult
    {
        public bool Succeeded { get; private set; }
        public bool Failed { get { return !Succeeded; } }
        public Token Token { get; private set; }

        public static IdentityResult Failure()
        {
            return new IdentityResult() { Succeeded = false };
        }

        public static IdentityResult Success(Token token)
        {
            return new IdentityResult() { Succeeded = true, Token = token };
        }
    }
}
