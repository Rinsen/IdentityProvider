using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProvider;
using Rinsen.IdentityProvider.ExternalApplications;
using Rinsen.IdentityProvider.Contracts.v1;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using Rinsen.IdentityProvider.Contracts;
using System.Linq;

namespace Rinsen.IdentityProviderWeb.Areas.Api
{
    [Route("api/v1/[controller]")]
    public class IdentityController : Controller
    {
        private readonly IIdentityAttributeStorage _identityAttributeStorage;
        private readonly IExternalApplicationService _externalApplicationService;
        private readonly IIdentityService _identityService;

        public IdentityController(IExternalApplicationService externalApplicationService,
            IIdentityService identityService,
            IIdentityAttributeStorage identityAttributeStorage)
        {
            _externalApplicationService = externalApplicationService;
            _identityService = identityService;
            _identityAttributeStorage = identityAttributeStorage;
        }

        [Route("[action]")]
        public async Task<ExternalIdentity> Get(string authToken, string applicationKey)
        {
            var identityResult = await _externalApplicationService.GetTokenAsync(authToken, applicationKey);

            if (identityResult.Failed)
            {
                throw new AuthenticationException($"Authentication failed for token id {authToken} and application key {applicationKey}");
            }

            var identity = await _identityService.GetIdentityAsync(identityResult.Token.IdentityId);

            var identityAttributes = await _identityAttributeStorage.GetIdentityAttributesAsync(identity.IdentityId);

            var extensions = new List<Extension>();

            if (identityAttributes.Any(attr => attr.Attribute == "Administrator"))
            {
                extensions.Add(new Extension { Type = RinsenIdentityConstants.Role, Value = RinsenIdentityConstants.Administrator });
            }

            return new ExternalIdentity
            {
                GivenName = identity.GivenName,
                IdentityId = identity.IdentityId,
                Surname = identity.Surname,
                Email = identity.Email,
                PhoneNumber = identity.PhoneNumber,
                Issuer = RinsenIdentityConstants.RinsenIdentityProvider,
                Expiration = identityResult.Token.Expiration,
                CorrelationId = identityResult.Token.CorrelationId,
                Extensions = extensions
            };
        }
    }
}
