namespace Rinsen.IdentityProvider.Sessions
{
    public interface ISessionHandler
    {
        /// <summary>
        /// Create new SessionId for identity session
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns>Session id</returns>
        string CreateSession(string loginId, string password);
        void DeleteSession();
    }
}
