using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LWFinancial.Helpers
{
    public class AccountHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Account> ListHouseholdAccounts(int householdId)
        {
            Household house = db.Households.Find(householdId);

            var accounts = house.Accounts.ToList();

            return accounts;
        }
    }
}