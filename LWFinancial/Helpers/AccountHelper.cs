using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using LWFinancial.Enumerations;

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

        public void UpdateAccountIncome(int accountId, decimal desiredAmount)
        {
            Account account = db.Accounts.Find(accountId);
            if (account.CurrentBalance == null)
            {
                account.CurrentBalance = 0;
                account.CurrentBalance = account.InitialBalance + desiredAmount;
            }
            else
            {
                account.CurrentBalance = account.CurrentBalance + desiredAmount;
            }

            db.SaveChanges();
        }

        public void UpdateAccountWithdrawlIncome(int accountId, decimal desiredAmount)
        {
            Account account = db.Accounts.Find(accountId);
            if (account.CurrentBalance == null)
            {
                account.CurrentBalance = 0;
                account.CurrentBalance = account.InitialBalance - desiredAmount;
            }
            else
            {
                account.CurrentBalance = account.CurrentBalance - desiredAmount;
            }

            db.SaveChanges();
        }

        public void UpdateReconciledIncome(int? accountId, decimal? reconciledAmount)
        {
            Account account = db.Accounts.Find(accountId);
            if (account.ReconciledBalance == null)
            {
                account.ReconciledBalance = 0;
                account.ReconciledBalance = account.ReconciledBalance + reconciledAmount;
            }
            else
            {
                account.ReconciledBalance = account.ReconciledBalance + reconciledAmount;
            }

            db.SaveChanges();
        }

        public void UpdateReconciledWithdrawlIncome(int? accountId, decimal? reconciledAmount)
        {
            Account account = db.Accounts.Find(accountId);

            account.ReconciledBalance = account.ReconciledBalance - reconciledAmount;

            db.SaveChanges();
        }
    }
}