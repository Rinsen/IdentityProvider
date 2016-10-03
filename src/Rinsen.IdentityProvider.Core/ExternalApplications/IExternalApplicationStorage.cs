using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface IExternalApplicationStorage
    {
        Task CreateAsync(ExternalApplication externalApplication);
        Task<ExternalApplication> GetAsync(string host);
        Task<IEnumerable<ExternalApplication>> GetAllAsync();
        Task UpdateAsync(ExternalApplication externalApplication);
        Task DeleteAsync(ExternalApplication externalApplication);
    }
}