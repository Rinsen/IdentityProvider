using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface IExternalApplicationStorage
    {
        Task CreateAsync(ExternalApplication localAccount);
        Task<ExternalApplication> GetAsync(string host);
        Task<IEnumerable<ExternalApplication>> GetAllAsync();
        Task UpdateAsync(ExternalApplication localAccount);
        Task DeleteAsync(ExternalApplication localAccount);
    }
}