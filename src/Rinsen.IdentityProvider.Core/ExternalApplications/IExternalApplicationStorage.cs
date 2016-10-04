﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface IExternalApplicationStorage
    {
        Task CreateAsync(ExternalApplication externalApplication);
        Task<ExternalApplication> GetFromHostAsync(string host);
        Task<ExternalApplication> GetFromApplicationKeyAsync(string applicationKey);
        Task<IEnumerable<ExternalApplication>> GetAllAsync();
        Task UpdateAsync(ExternalApplication externalApplication);
        Task DeleteAsync(ExternalApplication externalApplication);
    }
}