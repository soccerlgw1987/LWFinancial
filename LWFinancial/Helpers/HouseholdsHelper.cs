using LWFinancial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class HouseholdsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<ApplicationUser> UsersInHousehold(int householdId)
        {
            return db.Households.Find(householdId).ApplicationUsers;
        }
    }
}