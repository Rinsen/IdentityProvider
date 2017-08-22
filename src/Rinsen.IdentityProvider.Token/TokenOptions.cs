using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Rinsen.IdentityProvider.Core;

namespace Rinsen.IdentityProvider.Token
{
    public class TokenOptions : AuthenticationSchemeOptions, IOptions<TokenOptions>
    {
        private readonly LocalIdentityForReferenceHandler _localIdentityForReferenceHandler;

        public TokenOptions()
        {
            Events = new TokenAuthenticationEvents();
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

        public new ITokenAuthenticationEvents Events
        {
            get { return (ITokenAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }

        public string ApplicationKey { get; set; }
        public string ValidateTokenPath { get; set; }
        public string LoginPath { get; set; }

        public TokenOptions Value { get { return this; } }

        public string AuthenticationScheme { get; set; } = "Cookies";
        public string ReturnUrlParamterName { get; set; } = "ReturnUrl";
        public string ApplicationKeyParameterName { get; set; } = "ApplicationKey";
        public string TokenParameterName { get; set; } = "AuthToken";
        public string HostParameterName { get; set; } = "Host";

    }
}
