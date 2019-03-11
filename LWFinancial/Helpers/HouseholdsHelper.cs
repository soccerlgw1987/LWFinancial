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

        public void UpdateHouseholdIncome(int householdId, decimal desiredAmount)
        {
            Household house = db.Households.Find(householdId);
            if(house.CurrentBudgetAmount == null)
            {
                house.CurrentBudgetAmount = 0;
                house.CurrentBudgetAmount = house.CurrentBudgetAmount + desiredAmount;
            }
            else
            {
                house.CurrentBudgetAmount = house.CurrentBudgetAmount + desiredAmount;
            }
            
            db.SaveChanges();
        }

        public void UpdateHouseholdDeleteIncome(int householdId, decimal desiredAmount)
        {
            Household house = db.Households.Find(householdId);
            house.CurrentBudgetAmount = house.CurrentBudgetAmount - desiredAmount;

            db.SaveChanges();
        }

        public int GetMemberCountTotal()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var members = house.ApplicationUsers.Count();

            return members;
        }

        public int GetBudgetCountTotal()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var budget = house.Budgets.Count();

            return budget;
        }

        public int GetBudgetItemCountTotal()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var budgetItem = house.Budgets;
            var count = 0;

            foreach(var item in budgetItem)
            {
                count += item.BudgetItems.Count();
            }

            return count;
        }

        public int GetAccountCountTotal()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var accounts = house.Accounts.Count();

            return accounts;
        }

        public int GetTransactionCountTotal()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var accounts = house.Accounts;
            var count = 0;

            foreach (var item in accounts)
            {
                count += item.Transactions.Count();
            }

            return count;
        }

        public decimal GetEasyPieChartIncomePercent()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            decimal income = house.IncomeAmount;
            var budget = house.Budgets;
            decimal budgetTotal = 0;

            foreach (var item in budget)
            {
                budgetTotal += item.DesiredAmount;
            }

            decimal percent = decimal.Divide(budgetTotal, income) * 100;

            return percent;
        }

        public decimal GetEasyPieChartBudgetPercent()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var budget = house.Budgets;
            decimal budgetTotal = 0;
            decimal budgetItemTotal = 0;

            foreach (var item in budget)
            {
                budgetTotal += item.DesiredAmount;
                foreach(var budgetItem in item.BudgetItems)
                {
                    budgetItemTotal += budgetItem.DesiredAmount;
                }
            }

            decimal percent = decimal.Divide(budgetItemTotal, budgetTotal) * 100;

            return percent;
        }

        public decimal GetBudgetDesiredAmount()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var budget = house.Budgets;
            decimal budgetTotal = 0;

            foreach (var item in budget)
            {
                budgetTotal += item.DesiredAmount;
            }

            return budgetTotal;
        }

        public decimal GetBudgetItemDesiredAmount()
        {
            Household house = db.Households.Find(ListUserHousehold(HttpContext.Current.User.Identity.GetUserId()));

            var budget = house.Budgets;
            decimal budgetItemTotal = 0;

            foreach (var item in budget)
            {
                foreach (var budgetItem in item.BudgetItems)
                {
                    budgetItemTotal += budgetItem.DesiredAmount;
                }
            }

            return budgetItemTotal;
        }
    }
}