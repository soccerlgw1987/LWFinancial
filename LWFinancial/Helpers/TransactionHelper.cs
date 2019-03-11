using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class TransactionHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //LWTODO
        public ICollection<Transaction> ListTransactions(int householdId)
        {
            Account accounts = db.Accounts.FirstOrDefault(h => h.HouseholdId == householdId);

            var transactions = accounts.Transactions.ToList();

            return transactions;
        }
    }
}