using System;

namespace Rinsen.IdentityProvider.ExternalApplications
{
    public class IdentityResult
    {
        public bool Succeeded { get; private set; }
        public bool Failed { get { return !Succeeded; } }
        public Guid IdentityId { get; private set; }

        public static IdentityResult Failure()
        {
            return new IdentityResult() { Succeeded = false };
        }

        public static IdentityResult Success(Guid identity)
        {
            return new IdentityResult() { Succeeded = true, IdentityId = identity };
        }
    }
}
