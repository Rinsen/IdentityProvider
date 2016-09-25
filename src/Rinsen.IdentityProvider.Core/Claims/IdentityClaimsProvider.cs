//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Rinsen.IdentityProvider.Core.Claims
//{
//    public class IdentityClaimsProvider : IClaimsProvider
//    {
//        readonly IIdentityStorage _identityStorage;

//        public IdentityClaimsProvider(IIdentityStorage identityStorage)
//        {
//            _identityStorage = identityStorage;
//        }

//        public Task<IEnumerable<Claim>> GetClaimsAsync(Guid identityId)
//        {
//            var identity = await _identityStorage.GetAsync(identityId);

//            var claimsList = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, identity.FirstName + " " + identity.LastName),
//                new Claim(RinsenClaimTypes.IdentityId, identity.IdentityId.ToString()),
//            };

//            return claimsList;
//        }
//    }
//}
