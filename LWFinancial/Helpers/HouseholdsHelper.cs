using LWFinancial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class HouseholdsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int ListUserHousehold(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var household = user.HouseholdId;

            return household.GetValueOrDefault();
        }

        public ICollection<ApplicationUser> UsersInHousehold(int householdId)
        {
            return db.Households.Find(householdId).ApplicationUsers;
        }

        public bool IsUserInHousehold(string userId, int householdId)
        {
            var household = db.Households.Find(householdId);
            var flag = household.ApplicationUsers.Any(u => u.Id == userId);
            return (flag);
        }

        public void RemoveUserFromHousehold(string userId, int householdId)
        {
            if (IsUserInHousehold(userId, householdId))
            {
                Household house = db.Households.Find(householdId);
                var delUser = db.Users.Find(userId);
                house.ApplicationUsers.Remove(delUser);
                db.Entry(house).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}