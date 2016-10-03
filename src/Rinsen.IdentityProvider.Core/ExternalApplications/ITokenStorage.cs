using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core.ExternalApplications
{
    public interface ITokenStorage
    {
        Task CreateAsync(Token token);
        Task<Token> GetAndDeleteAsync(string tokenId);
    }
}
