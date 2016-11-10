using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Rinsen.IdentityProvider.Core;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : AuthenticationOptions, IOptions<TokenOptions>
    {
        private readonly LocalIdentityForReferenceHandler _localIdentityForReferenceHandler;

        public TokenOptions()
        {
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
            AuthenticationScheme = TokenDefaults.AuthenticationScheme;
            CookieAuthenticationScheme = "RinsenCookie";
            ReturnUrlParamterName = "ReturnUrl";
            ApplicationKeyParameterName = "ApplicationKey";
            HostParameterName = "Host";
            TokenParameterName = "AuthToken";
        }

        /// <summary>
        /// Create a LocalIdentityForReferenceHandler for trying to create identity in database when logging in if identity does not exist
        /// </summary>
        /// <param name="connectionString">Database where identity is created</param>
        public TokenOptions(string connectionString)
            :this()
        {
            _localIdentityForReferenceHandler = new LocalIdentityForReferenceHandler(connectionString);

            Events = new TokenAuthenticationEvents
            {
                OnAuthenticationSuccess = async context => { await _localIdentityForReferenceHandler.CreateReferenceIdentityIfNotExists(context.ClaimsPrincipal); }
            };
        }

        public string ApplicationKey { get; set; }
        public string ValidateTokenPath { get; set; }
        public string LoginPath { get; set; }
        public string CookieAuthenticationScheme { get; set; }
        public TokenOptions Value { get { return this; } }
        public string ReturnUrlParamterName { get; set; }
        public string ApplicationKeyParameterName { get; set; }
        public string TokenParameterName { get; set; }
        public string HostParameterName { get; set; }
        public ITokenAuthenticationEvents Events { get; set; } = new TokenAuthenticationEvents();

    }
}
