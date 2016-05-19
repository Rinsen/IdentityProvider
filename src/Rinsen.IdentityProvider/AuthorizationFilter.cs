//using System.Net;
//using System.Collections.Generic;
//using Microsoft.AspNet.Mvc;
//using Microsoft.AspNet.Mvc.Filters;
//using Microsoft.Extensions.Logging;

//namespace Rinsen.IdentityProvider
//{
//    public class AuthorizationFilter : AuthorizationFilterAttribute
//    {
//        readonly ILogger<AuthorizationFilter> _logger;

//        public AuthorizationFilter(ILogger<AuthorizationFilter> logger)
//        {
//            _logger = logger;
//        }

//        public override void OnAuthorization(AuthorizationContext context)
//        {
//            if (HasAllowAnonymous(context))
//            {
//                return;
//            }

//            if (!context.HttpContext.User.Identity.IsAuthenticated)
//            {
//                _logger.LogWarning("Not authenticated request to route {0} from ip address {1} redirect to login page", context.HttpContext.Request.Path.ToString(), context.HttpContext.GetClientIPAddressString());

//                var fullUri = context.HttpContext.Request.Path.ToString();
//                var fullUriEncoded = WebUtility.UrlEncode(fullUri);
//                context.Result = new RedirectToActionResult("Index", "User", new Dictionary<string, object> { { "returnUrl", fullUriEncoded } });
//            }

//            // Add some Authorization check ?
//        }
//    }
//}
