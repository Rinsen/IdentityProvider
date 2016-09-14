using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace Rinsen.IdentityProvider.Cookie
{
    public class CookieHandler : AuthenticationHandler<CookieOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return AuthenticateResult.Fail("No auth");
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            


            return base.HandleSignInAsync(context);
        }

    }
}
