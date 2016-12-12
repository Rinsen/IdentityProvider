using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProvider;
using Rinsen.IdentityProvider.ExternalApplications;
using Rinsen.IdentityProvider.Contracts.v1;
using System.Security.Authentication;
using System.Threading.Tasks;
using Rinsen.IdentityProviderWeb.IdentityExtensions;
using System.Collections.Generic;
using Rinsen.IdentityProvider.Contracts;

namespace Rinsen.IdentityProviderWeb.Areas.Api
{
    [Route("api/v1/[controller]")]
    public class IdentityController : Controller
    {
        private readonly AdministratorStorage _administratorStorage;
        private readonly IExternalApplicationService _externalApplicationService;
        private readonly IIdentityService _identityService;

        public IdentityController(IExternalApplicationService externalApplicationService,
            IIdentityService identityService,
            AdministratorStorage administratorStorage)
        {
            _externalApplicationService = externalApplicationService;
            _identityService = identityService;
            _administratorStorage = administratorStorage;
        }

        [Route("[action]")]
        public async Task<ExternalIdentity> Get(string authToken, string applicationKey)
        {
            var identityResult = await _externalApplicationService.GetIdentityForTokenAndApplicationKeyAsync(authToken, applicationKey);

            if (identityResult.Failed)
            {
                throw new AuthenticationException($"Authentication failed for token id {authToken} and application key {applicationKey}");
            }

            var identity = await _identityService.GetIdentityAsync(identityResult.IdentityId);

            var administrator = await _administratorStorage.GetAsync(identity.IdentityId);

            var extensions = new List<Extension>();

            if (administrator != default(Administrator))
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
                Extensions = extensions
            };
        }
    }
}
