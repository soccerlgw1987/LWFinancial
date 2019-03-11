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

        public ICollection<Transaction> ListTransactions(int householdId)
        {
            var household = db.Households.Find(householdId);

            var transactions = household.Accounts.SelectMany(b => b.Transactions);

            return transactions.ToList();
        }

        public void UpdateReconciledIncome(int? transactionId, decimal? reconciledAmount)
        {
            Transaction transaction = db.Transactions.Find(transactionId);
            if (transaction.ReconciledAmount == null)
            {
                transaction.ReconciledAmount = 0;
                transaction.ReconciledAmount = transaction.ReconciledAmount + reconciledAmount;
            }
            else
            {
                transaction.ReconciledAmount = transaction.ReconciledAmount + reconciledAmount;
            }

            db.SaveChanges();
        }

        public void UpdateReconciledWithdrawlIncome(int? transactionId, decimal? reconciledAmount)
        {
            Transaction transaction = db.Transactions.Find(transactionId);

            transaction.ReconciledAmount = transaction.ReconciledAmount - reconciledAmount;

            db.SaveChanges();
        }
    }
}