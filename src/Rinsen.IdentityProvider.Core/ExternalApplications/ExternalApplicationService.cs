using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplicationService : IExternalApplicationService
    {
        private readonly IExternalApplicationStorage _externalApplicationStorage;
        private readonly ITokenStorage _tokenStorage;
        private readonly IIdentityAccessor _identityAccessor;

        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public ExternalApplicationService(IExternalApplicationStorage externalApplicationStorage,
            ITokenStorage tokenStorage,
            IIdentityAccessor identityAccessor)
        {
            _externalApplicationStorage = externalApplicationStorage;
            _tokenStorage = tokenStorage;
            _identityAccessor = identityAccessor;
        }

        public async Task<ValidationResult> GetTokenForValidHostAsync(string returnUrl)
        {
            var uri = new Uri(returnUrl);

            var externalApplication = await _externalApplicationStorage.GetAsync(uri.Host);

            if (externalApplication == default(ExternalApplication))
            {
                return  ValidationResult.Failure();
            }

            var bytes = new byte[32];
            CryptoRandom.GetBytes(bytes);
            var tokenId = Base64UrlTextEncoder.Encode(bytes);

            var token = new Token
            {
                TokenId = tokenId,
                Created = DateTimeOffset.Now,
                ExternalApplicationId = externalApplication.ExternalApplicationId,
                IdentityId = _identityAccessor.IdentityId
            };

            await _tokenStorage.CreateAsync(token);

            return ValidationResult.Success(token.TokenId);
        }
    }
}
