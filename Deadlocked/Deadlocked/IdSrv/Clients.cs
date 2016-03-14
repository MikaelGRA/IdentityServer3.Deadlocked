using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace IdSrv
{
   static class Clients
   {
      public static List<Client> Get()
      {
         return new List<Client>
            {
                new Client
                {
                    ClientName = "Silicon on behalf of Carbon Client",
                    ClientId = "carbon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,

                    Flow = Flows.ResourceOwner,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("21B5F798-BE55-42BC-8AA8-0025B903DC3B".Sha256()) { Type = "SharedSecret" },
                    },

                     AbsoluteRefreshTokenLifetime = 315569260, // 10 years
                     RefreshTokenExpiration = TokenExpiration.Absolute,
                     EnableLocalLogin = true,

                     RefreshTokenUsage = TokenUsage.OneTimeOnly,
                     AllowClientCredentialsOnly = true,
                     AllowRememberConsent = true,

                     AllowAccessToAllScopes = true,
                     
                }
            };
      }
   }
}