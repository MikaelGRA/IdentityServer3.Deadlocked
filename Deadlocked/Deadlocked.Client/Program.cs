using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Deadlocked.Client
{
   class Program
   {
      static void Main( string[] args )
      {
         PerformStressTest().Wait();
      }

      static async Task PerformStressTest()
      {
         var client = new TokenClient(
            "http://localhost:55584/oauth/connect/token",
            "carbon",
            "21B5F798-BE55-42BC-8AA8-0025B903DC3B" );

         var response = await client.RequestResourceOwnerPasswordAsync( "mikael", "somePassword!", "openid profile email api1 offline_access" );
         string refreshToken = response.RefreshToken;

         while( true )
         {
            List<Task<TokenResponse>> tasks = new List<Task<TokenResponse>>();
            for( int i = 0 ; i < 5 ; i++ )
            {
               tasks.Add( client.RequestRefreshTokenAsync( refreshToken, null ) );
            }

            try
            {
               await Task.WhenAll( tasks );
            }
            catch( Exception )
            {
               // really dont care
            }

            var completedTask = tasks.First( x => x.IsCompleted && !x.Result.IsError );

            refreshToken = completedTask.Result.RefreshToken;

            await Task.Delay( 1000 );
         }
      }
   }
}
