using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public class ExternalApplicationService : IExternalApplicationService
    {
        private readonly IExternalApplicationStorage _externalApplicationStorage;
        private readonly ITokenStorage _tokenStorage;

        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public ExternalApplicationService(IExternalApplicationStorage externalApplicationStorage,
            ITokenStorage tokenStorage)
        {
            _externalApplicationStorage = externalApplicationStorage;
            _tokenStorage = tokenStorage;
        }

        public async Task<ValidationResult> GetTokenForValidHostAsync(string returnUrl, Guid identityId)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return ValidationResult.Failure();

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
                IdentityId = identityId
            };

            await _tokenStorage.CreateAsync(token);

            return ValidationResult.Success(token.TokenId);
        }
    }
}
