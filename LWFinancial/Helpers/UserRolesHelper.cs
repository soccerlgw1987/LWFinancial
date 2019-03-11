using LWFinancial.Enumerations;
using LWFinancial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = new ApplicationUser();

        public bool IsUserInRole(string userId, RoleNames roleName)
        {
            return userManager.IsInRole(userId, roleName.ToString());
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, RoleNames roleName)
        {
            var result = userManager.AddToRole(userId, roleName.ToString());
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public string UserInRole(RoleNames roleName)
        {
            var result = "";
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    result = user.Id;
            }
            return result;
        }
    }
}