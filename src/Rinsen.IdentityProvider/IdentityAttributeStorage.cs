using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider
{
    public class IdentityAttributeStorage : IIdentityAttributeStorage
    {
        public async Task<IEnumerable<IdentityAttribute>> GetIdentityAttributesAsync(Guid identityId)
        {

            throw new NotImplementedException();
        }
    }
}
