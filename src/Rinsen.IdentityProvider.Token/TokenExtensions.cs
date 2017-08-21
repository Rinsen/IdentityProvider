//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.Authentication;

//namespace Rinsen.IdentityProvider.Token
//{
//    public class TokenExtensions
//    {

//        public virtual AuthenticationBuilder AddScheme<TOptions, THandler>(string authenticationScheme, string displayName, Action<TOptions> configureOptions)
//           where TOptions : AuthenticationSchemeOptions, new()
//           where THandler : AuthenticationHandler<TOptions>
//        {
//            Services.Configure<AuthenticationOptions>(o =>
//            {
//                o.AddScheme(authenticationScheme, scheme => {
//                    scheme.HandlerType = typeof(THandler);
//                    scheme.DisplayName = displayName;
//                });
//            });
//            if (configureOptions != null)
//            {
//                Services.Configure(authenticationScheme, configureOptions);
//            }
//            Services.AddTransient<THandler>();
//            return this;
//        }
//    }
//}

