using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ExternalApplicationService> _log;

        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public ExternalApplicationService(IExternalApplicationStorage externalApplicationStorage,
            ITokenStorage tokenStorage,
            ILogger<ExternalApplicationService> log)
        {
            _externalApplicationStorage = externalApplicationStorage;
            _tokenStorage = tokenStorage;
            _log = log;
        }

        public async Task<IdentityResult> GetIdentityForTokenAndApplicationKeyAsync(string tokenId, string applicationKey)
        {
            if (string.IsNullOrEmpty(tokenId))
            {
                throw new ArgumentException("TokenId is required", nameof(tokenId));
            }

            if (string.IsNullOrEmpty(applicationKey))
            {
                throw new ArgumentException("Application key is required", nameof(applicationKey));
            }

            var token = await _tokenStorage.GetAndDeleteAsync(tokenId);

            if (token == default(Token))
            {
                _log.LogWarning($"Invalid token id {tokenId}");
                return IdentityResult.Failure();
            }

            var externalApplication = await _externalApplicationStorage.GetFromApplicationKeyAsync(applicationKey);

            if (externalApplication == default(ExternalApplication))
            {
                _log.LogWarning($"Invalid application key {applicationKey}");
                return IdentityResult.Failure();
            }

            if (externalApplication.Active 
                && externalApplication.ExternalApplicationId == token.ExternalApplicationId 
                && token.Created.AddSeconds(15) >= DateTimeOffset.Now)
            {
                return IdentityResult.Success(token.IdentityId);
            }

            return IdentityResult.Failure();
        }

        public async Task<ValidationResult> GetTokenForValidHostAsync(string host, Guid identityId)
        {
            if (string.IsNullOrEmpty(host))
                return ValidationResult.Failure();

            var externalApplication = await _externalApplicationStorage.GetFromHostAsync(host);

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
