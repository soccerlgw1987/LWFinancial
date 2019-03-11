using LWFinancial.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LWFinancial.Enumerations;

namespace LWFinancial.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdsHelper householdHelper = new HouseholdsHelper();
        private UserRolesHelper userRolesHelper = new UserRolesHelper();

        public void GetOverdraftNotification(int householdId, string accountName)
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            Household house = db.Households.Find(householdId);
            var headOfHousehold = "";

            foreach (var item in house.ApplicationUsers)
            {
                if(userRolesHelper.IsUserInRole(item.Id, RoleNames.HOH))
                {
                    headOfHousehold = item.Id;
                    break;
                }
            }

            var newNotification = new HouseholdNotification
            {
                HouseholdId = householdId,
                Created = DateTime.Now
            };

            newNotification.RecipientId = headOfHousehold;
            newNotification.Title = "Overdraft Warning!";
            newNotification.Decscription = $"{currentUser} has caused an overdraft in account {accountName}";
            db.HouseholdNotifications.Add(newNotification);

            db.SaveChanges();
        }

        public void GetLowBalanceNotification(int householdId, string accountName)
        {
            var currentUser = HttpContext.Current.User.Identity.Name;
            Household house = db.Households.Find(householdId);
            var headOfHousehold = "";

            foreach (var item in house.ApplicationUsers)
            {
                if (userRolesHelper.IsUserInRole(item.Id, RoleNames.HOH))
                {
                    headOfHousehold = item.Id;
                    break;
                }
            }

            var newNotification = new HouseholdNotification
            {
                HouseholdId = householdId,
                Created = DateTime.Now
            };

            newNotification.RecipientId = headOfHousehold;
            newNotification.Title = "Low Balance Alert!";
            newNotification.Decscription = $"{currentUser} has caused account {accountName} to be in low balance!";
            db.HouseholdNotifications.Add(newNotification);

            db.SaveChanges();
        }

        public int GetUnreadNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.HouseholdNotifications.Where(n => n.RecipientId == userId && n.Read == false).Count();
        }

        public int GetNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.HouseholdNotifications.Where(n => n.RecipientId == userId).Count();
        }

        public List<HouseholdNotification> GetNewNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.HouseholdNotifications.Where(n => n.RecipientId == userId && n.Read == false).OrderByDescending(n => n.Created).ToList();
        }
    }
}