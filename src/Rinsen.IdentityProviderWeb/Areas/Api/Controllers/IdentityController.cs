using Microsoft.AspNetCore.Mvc;
using Rinsen.IdentityProviderWeb.Areas.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rinsen.IdentityProviderWeb.Areas.Api
{
    [Area("api")]
    public class IdentityController : Controller
    {


        public IdentityController()
        {

        }

        public IdentityResult Get(string token, string applicationKey)
        {


            return new IdentityResult();
        }


    }
}
