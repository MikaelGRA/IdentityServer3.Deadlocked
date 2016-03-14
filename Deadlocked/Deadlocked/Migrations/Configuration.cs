namespace Deadlocked.Migrations
{
   using System;
   using System.Data.Entity;
   using System.Data.Entity.Migrations;
   using System.Linq;
   using Microsoft.AspNet.Identity;
   using Models;
   internal sealed class Configuration : DbMigrationsConfiguration<Deadlocked.Models.ApplicationDbContext>
   {
      public Configuration()
      {
         AutomaticMigrationsEnabled = false;
      }

      protected override void Seed( Deadlocked.Models.ApplicationDbContext context )
      {
         //  This method will be called after migrating to the latest version.

         //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
         //  to avoid creating duplicate seed data. E.g.
         //

         context.Users.AddOrUpdate(
           p => p.UserName,
           new ApplicationUser
           {
              Email = "some@email.somewhere",
              EmailConfirmed = true,
              PasswordHash = new PasswordHasher().HashPassword( "somePassword!" ),
              SecurityStamp = Guid.NewGuid().ToString(),
              UserName = "mikael",
           }
         );

      }
   }
}
