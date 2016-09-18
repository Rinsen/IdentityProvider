using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProvider.Core
{
    public class CreateIdentityResult
    {
        public Identity Identity { get; set; }
        public bool Succeeded { get; set; }
    }
}
