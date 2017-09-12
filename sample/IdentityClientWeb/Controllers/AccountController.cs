using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClientWeb.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public void Login()
        {
            HttpContext.ChallengeAsync(Rinsen.IdentityProvider.Token.TokenDefaults.AuthenticationScheme);

        }

    }
}