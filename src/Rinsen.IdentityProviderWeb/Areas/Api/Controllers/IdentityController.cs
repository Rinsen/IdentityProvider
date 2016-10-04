using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProvider.Core;
using Rinsen.IdentityProvider.Core.ExternalApplications;
using Rinsen.IdentityProviderWeb.Areas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Areas.Api
{
    [Area("api")]
    public class IdentityController : Controller
    {
        private readonly IExternalApplicationService _externalApplicationService;
        private readonly IIdentityService _identityService;

        public IdentityController(IExternalApplicationService externalApplicationService, IIdentityService identityService)
        {
            _externalApplicationService = externalApplicationService;
            _identityService = identityService;
        }

        public async Task<ExternalIdentity> Get(string token, string applicationKey)
        {
            var identityResult = await _externalApplicationService.GetIdentityForTokenAndApplicationKeyAsync(token, applicationKey);

            if (identityResult.Failed)
            {
                throw new AuthenticationException($"Authentication failed for token id {token} and application key {applicationKey}");
            }

            var identity = await _identityService.GetIdentityAsync(identityResult.IdentityId);

            return new ExternalIdentity
            {
                GivenName = identity.GivenName,
                IdentityId = identity.IdentityId,
                Surname = identity.Surname,
                Issuer = "RinsenIdentityProvider"
            };
        }
    }
}
