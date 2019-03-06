using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class AccountEditHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static AccountEdit GetAccountEditData(int accountId)
        {
            Account account = db.Accounts.Find(accountId);
            return new AccountEdit
            {
                Accounts = account
            };
        }
    }
}