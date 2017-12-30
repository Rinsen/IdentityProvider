using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProviderWeb.Areas.WebApi.Models;
using Rinsen.IdentityProvider.ExternalApplications;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Rinsen.IdentityProviderWeb.Areas.WebApi.Controllers
{
    [Area("webapi")]
    [Authorize("AdminsOnly")]
    public class ExternalApplicationsController : Controller
    {
        private readonly IExternalApplicationStorage _externalApplicationStorage;

        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public ExternalApplicationsController(
            IExternalApplicationStorage externalApplicationStorage)
        {
            _externalApplicationStorage = externalApplicationStorage;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<ExternalApplicationsResult> GetAll()
        {
            var result = new ExternalApplicationsResult();

            result.ExternalApplications = await _externalApplicationStorage.GetAllAsync();

            return result;
        }

        [HttpPost]
        public async Task<ExternalApplicationsResult> Create([FromBody]ExternalApplicationToCreate externalApplicationToCreate)
        {
            var bytes = new byte[60];
            CryptoRandom.GetBytes(bytes);
            var applicationKey = Base64UrlTextEncoder.Encode(bytes);

            var newExternalApplication = new ExternalApplication
            {
                Active = externalApplicationToCreate.Active,
                ActiveUntil = externalApplicationToCreate.ActiveUntil,
                ApplicationKey = applicationKey,
                ExternalApplicationId = Guid.NewGuid(),
                Name = externalApplicationToCreate.HostName
            };

            await _externalApplicationStorage.CreateAsync(newExternalApplication);

            return new ExternalApplicationsResult { ExternalApplications = new[] { newExternalApplication } };
        }

        [HttpPost]
        public async Task<ExternalApplicationsResult> Update([FromBody]ExternalApplicationToUpdate externalApplicationToUpdate)
        {
            var externalApplication = await _externalApplicationStorage.GetFromExternalApplicationIdAsync(externalApplicationToUpdate.ExternalApplicationId);

            externalApplication.Active = externalApplicationToUpdate.Active;
            externalApplication.ActiveUntil = externalApplicationToUpdate.ActiveUntil;
            externalApplication.Name = externalApplicationToUpdate.Hostname;

            await _externalApplicationStorage.UpdateAsync(externalApplication);

            return new ExternalApplicationsResult { ExternalApplications = Enumerable.Empty<ExternalApplication>() };
        }
    }
}
