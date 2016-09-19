using System;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public interface IIdentityStorage
    {
        Task CreateAsync(Identity identity);
        Identity Get(Guid identityId);
    }
}