using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Rinsen.IdentityProvider.Core
{
    public class RinsenDefaultCookieAuthenticationOptions : CookieAuthenticationOptions
    {
        public RinsenDefaultCookieAuthenticationOptions(string connectionString)
        {
            AutomaticAuthenticate = true;
            SessionStore = new SqlTicketStore(new SessionStorage(connectionString));
            AuthenticationScheme = "RinsenCookie";
            CookieSecure = CookieSecurePolicy.Always;
            
        }
    }
}
