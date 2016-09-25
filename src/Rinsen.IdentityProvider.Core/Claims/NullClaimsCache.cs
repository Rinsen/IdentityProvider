//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;

//namespace Rinsen.IdentityProvider.Core.Claims
//{
//    public class NullClaimsCache : IClaimsCache
//    {
//        public void Add(IEnumerable<Claim> claims)
//        {
//            return;
//        }

//        public bool TryGet(Guid identityId, out IEnumerable<Claim> claims)
//        {
//            claims = Enumerable.Empty<Claim>();
//            return false;
//        }
//    }
//}
