using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Deadlocked.Models;
using IdentityServer3.Core.Configuration;
using IdentityServer3.AccessTokenValidation;
using IdSrv;
using IdentityServer3.Core.Services;
using IdentityServer3.AspNetIdentity;
using System.Data.Entity;

namespace Deadlocked
{
   public partial class Startup
   {
      public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

      public static string PublicClientId { get; private set; }

      // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
      public void ConfigureAuth( IAppBuilder app )
      {
         // Configure the db context and user manager to use a single instance per request
         app.CreatePerOwinContext( ApplicationDbContext.Create );
         app.CreatePerOwinContext<ApplicationUserManager>( ApplicationUserManager.Create );

         // Enable the application to use a cookie to store information for the signed in user
         // and to use a cookie to temporarily store information about a user logging in with a third party login provider
         app.UseCookieAuthentication( new CookieAuthenticationOptions() );

         app.Map( "/oauth", idsrvApp =>
         {
            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryScopes( Scopes.Get() );
            factory.UseInMemoryClients( Clients.Get() );

            factory.Register( new Registration<DbContext, ApplicationDbContext>() );
            factory.Register( new Registration<UserManager<ApplicationUser, string>, ApplicationUserManager>() );
            factory.Register( new Registration<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>() );

            factory.UserService = new Registration<IUserService, AspNetIdentityUserService<ApplicationUser, string>>();

            idsrvApp.UseIdentityServer( new IdentityServerOptions
            {
               LoggingOptions = new LoggingOptions()
               {
                  EnableHttpLogging = true,
                  EnableKatanaLogging = true,
                  EnableWebApiDiagnostics = true,
                  WebApiDiagnosticsIsVerbose = true
               },
               SiteName = "OAuth2 Manager",
               SigningCertificate = Certificate.Load(),
               IssuerUri = "http://localhost:55584",

               Factory = factory,
               RequireSsl = false,
            } );
         } );

         // Accepting OAuth2 tokens as authorizing input
         app.UseIdentityServerBearerTokenAuthentication( new IdentityServerBearerTokenAuthenticationOptions
         {
            Authority = "http://localhost:55584/oauth",
            ValidationMode = ValidationMode.Local,
            SigningCertificate = Certificate.Load(),
            IssuerName = "http://localhost:55584",
         } );
      }
   }
}
