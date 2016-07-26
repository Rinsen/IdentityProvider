using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using Rinsen.IdentityProvider.Core.Sessions;
using Rinsen.IdentityProvider.Core.Claims;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rinsen.IdentityProvider.Core.LocalAccounts;

namespace Rinsen.IdentityProvider.Core
{
    public static class ExtensionMethods
    {
        public static void AddRinsenIdentityCore(this IServiceCollection services, Action<IdentityOptions> identityOptionsAction)
        {
            var identityOptions = new IdentityOptions();

            identityOptionsAction.Invoke(identityOptions);

            services.AddSingleton(identityOptions);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ISessionHandler, SessionHandler>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<PasswordHashGenerator, PasswordHashGenerator>();
            services.AddTransient<IRandomDataGenerator, RandomDataGenerator>();
            services.AddTransient<IIdentityAccessor, IdentityAccessService>();
            services.AddTransient<IdentityProvider, IdentityProvider>();
            services.AddTransient<IClaimsPrincipalHandler, ClaimsPrincipalHandler>();
            services.AddTransient<ClaimsProvidersFactory, ClaimsProvidersFactory>();
            services.AddTransient<IdentityClaimsProvider, IdentityClaimsProvider>();
            services.AddSingleton<IClaimsCache, NullClaimsCache>();
            services.AddTransient<ILocalAccountService, LocalAccountService>();

        }

        public static string GetClaimStringValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.GetClaimStringValue(m => m.Type == claimType);
        }

        public static string GetClaimStringValue(this ClaimsPrincipal claimsPrincipal, Predicate<Claim> match)
        {
            if (claimsPrincipal.HasClaim(match))
            {
                try
                {
                    return claimsPrincipal.Claims.Where(new Func<Claim, bool>(match)).Single().Value;
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException("The claims collection does not contain exactly one element.");
                }
                
            }
            else
            {
                throw new InvalidOperationException("The claims collection does not contain a element that match.");
            }
        }

        public static int GetClaimIntValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.GetClaimIntValue(m => m.Type == claimType);
        }

        public static int GetClaimIntValue(this ClaimsPrincipal claimsPrincipal, Predicate<Claim> match)
        {
            int result;
            if(!int.TryParse(claimsPrincipal.GetClaimStringValue(match), out result))
            {
                throw new InvalidOperationException("Parse exception in claims value");
            }

            return result;
        }

        public static Guid GetClaimGuidValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.GetClaimGuidValue(m => m.Type == claimType);
        }

        public static Guid GetClaimGuidValue(this ClaimsPrincipal claimsPrincipal, Predicate<Claim> match)
        {
            Guid result;
            if (!Guid.TryParse(claimsPrincipal.GetClaimStringValue(match), out result))
            {
                throw new InvalidOperationException("Parse exception in claims value");
            }

            return result;
        }

        public static string GetClientIPAddressString(this HttpContext context)
        {
            return context.GetClientIPAddress().ToString();
        }

        public static IPAddress GetClientIPAddress(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var remoteIp = GetRemoteIP(context);

            IPAddress address;
            if(IPAddress.TryParse(remoteIp, out address))
            {
                return address;
            }

            var httpConnectionFeature = context.Features.Get<IHttpConnectionFeature>();

            if (httpConnectionFeature == null)
            {
                throw new NullReferenceException("HttpConnectionFeature is null and no forwarded remote address is found in headers");
            }

            return httpConnectionFeature.RemoteIpAddress;
        }

        static string GetRemoteIP(HttpContext context)
        {
            string[] remoteIpHeaders =
                    {
                "X-FORWARDED-FOR",
                "REMOTE_ADDR",
                "HTTP_X_FORWARDED_FOR",
                "HTTP_CLIENT_IP",
                "HTTP_X_FORWARDED",
                "HTTP_X_CLUSTER_CLIENT_IP",
                "HTTP_FORWARDED_FOR",
                "HTTP_FORWARDED",
                "X_FORWARDED_FOR",
                "CLIENT_IP",
                "X_FORWARDED",
                "X_CLUSTER_CLIENT_IP",
                "FORWARDED_FOR",
                "FORWARDED"
            };

            string value;
            foreach (string remoteIpHeader in remoteIpHeaders)
            {
                if (context.Request.Headers.ContainsKey(remoteIpHeader))
                {
                    value = context.Request.Headers[remoteIpHeader];
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Split(',')[0].Split(';')[0];
                        if (value.Contains("="))
                        {
                            value = value.Split('=')[1];
                        }
                        value = value.Trim('"');
                        if (value.Contains(":"))
                        {
                            value = value.Substring(0, value.LastIndexOf(':'));
                        }
                        return value.TrimStart('[').TrimEnd(']');
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return null;
        }
    }
}
