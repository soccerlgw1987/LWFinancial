namespace LWFinancial.Migrations
{
    using LWFinancial.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LWFinancial.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LWFinancial.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }
            if (!context.Roles.Any(r => r.Name == "Delegate"))
            {
                roleManager.Create(new IdentityRole { Name = "Delegate" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Add the admin user
            if (!context.Users.Any(u => u.Email == "soccerlgw1987@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "soccerlgw1987@yahoo.com",
                    Email = "soccerlgw1987@yahoo.com",
                    FirstName = "Landon",
                    LastName = "Wyant",
                    FullName = "Landon Wyant",
                    DisplayName = "Landon"
                },
                "test87");
            }
        }
    }
}
