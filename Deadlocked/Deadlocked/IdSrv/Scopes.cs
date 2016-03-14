using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace IdSrv
{
   static class Scopes
   {
      public static List<Scope> Get()
      {
         var scopes = new List<Scope>
            {
                new Scope
                {
                    Name = "api1"
                }
            };

         scopes.AddRange( StandardScopes.All );
         scopes.Add( StandardScopes.OfflineAccess );

         return scopes;
      }
   }
}