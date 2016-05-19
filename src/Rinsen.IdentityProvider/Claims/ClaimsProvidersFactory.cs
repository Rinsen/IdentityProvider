using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace Rinsen.IdentityProvider.Claims
{
    public class ClaimsProvidersFactory
    {
        readonly IdentityOptions _options;
        readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsProvidersFactory(IHttpContextAccessor httpContextAccesor, IdentityOptions options)
        {
            _options = options;
            _httpContextAccessor = httpContextAccesor;
        }

        internal IEnumerable<IClaimsProvider> GetClaimsProviders()
        {
            List<IClaimsProvider> claimsProviders = new List<IClaimsProvider>();

            foreach (var claimProviderType in _options.ClaimsProviders)
            {
                var claimProvider = (IClaimsProvider)_httpContextAccessor.HttpContext.RequestServices.GetService(claimProviderType.Type);

                if (claimProvider == null)
                {
                    throw new NullReferenceException(string.Format("No service found for type {0} in claims provider factory", claimProviderType.Type));
                }

                claimsProviders.Add(claimProvider);
            }

            return claimsProviders;
        }
    }
}