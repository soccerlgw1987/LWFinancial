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
            if (!context.Roles.Any(r => r.Name == "HOH"))
            {
                roleManager.Create(new IdentityRole { Name = "HOH" });
            }
            if (!context.Roles.Any(r => r.Name == "Guest"))
            {
                roleManager.Create(new IdentityRole { Name = "Guest" });
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

            var userId = userManager.FindByEmail("soccerlgw1987@yahoo.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "yijing@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "yijing@mailinator.com",
                    Email = "yijing@mailinator.com",
                    FirstName = "Yijing",
                    LastName = "Wyant",
                    FullName = "Yijing Wyant",
                    DisplayName = "Teng"
                },
                "test87");
            }

            userId = userManager.FindByEmail("yijing@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "ella@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ella@mailinator.com",
                    Email = "ella@mailinator.com",
                    FirstName = "Ella",
                    LastName = "Wyant",
                    FullName = "Ella Wyant",
                    DisplayName = "Wife"
                },
                "test87");
            }

            userId = userManager.FindByEmail("ella@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "emma@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "emma@mailinator.com",
                    Email = "emma@mailinator.com",
                    FirstName = "Emma",
                    LastName = "Wyant",
                    FullName = "Emma Wyant",
                    DisplayName = "Child"
                },
                "test87");
            }

            userId = userManager.FindByEmail("emma@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "tara@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "tara@mailinator.com",
                    Email = "tara@mailinator.com",
                    FirstName = "Tara",
                    LastName = "Wyant",
                    FullName = "Tara Wyant",
                    DisplayName = "The_Cat"
                },
                "test87");
            }

            userId = userManager.FindByEmail("tara@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "wayne@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "wayne@mailinator.com",
                    Email = "wayne@mailinator.com",
                    FirstName = "Wayne",
                    LastName = "Turner",
                    FullName = "Wayne Turner",
                    DisplayName = "Bloodhog"
                },
                "test87");
            }

            userId = userManager.FindByEmail("wayne@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "casey@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "casey@mailinator.com",
                    Email = "casey@mailinator.com",
                    FirstName = "Casey",
                    LastName = "Anderson",
                    FullName = "Casey Anderson",
                    DisplayName = "Friend"
                },
                "test87");
            }

            userId = userManager.FindByEmail("casey@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");

            if (!context.Users.Any(u => u.Email == "sandy@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sandy@mailinator.com",
                    Email = "sandy@mailinator.com",
                    FirstName = "Sandy",
                    LastName = "Turner",
                    FullName = "Sandy Turner",
                    DisplayName = "Cousin"
                },
                "test87");
            }

            userId = userManager.FindByEmail("sandy@mailinator.com").Id;
            userManager.AddToRole(userId, "Guest");
        }
    }
}
