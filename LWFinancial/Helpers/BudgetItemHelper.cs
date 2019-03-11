using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LWFinancial.Helpers
{
    public class BudgetItemHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //LWTODO
        public ICollection<BudgetItem> ListBudgetItems(int householdId)
        {
            Budget budgets = db.Budgets.FirstOrDefault(h => h.HouseholdId == householdId);

            var budgetitems = budgets.BudgetItems.ToList();

            return budgetitems;
        }

        public void UpdateBudgetItemIncome(int? budgetItemId, decimal desiredAmount)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(budgetItemId);
            if (budgetItem.CurrentAmount == null)
            {
                budgetItem.CurrentAmount = 0;
                budgetItem.CurrentAmount = budgetItem.CurrentAmount + desiredAmount;
            }
            else
            {
                budgetItem.CurrentAmount = budgetItem.CurrentAmount + desiredAmount;
            }

            db.SaveChanges();
        }

        public void UpdateBudgetItemWithdrawlIncome(int? budgetItemId, decimal desiredAmount)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(budgetItemId);
            if (budgetItem.CurrentAmount == null)
            {
                budgetItem.CurrentAmount = 0;
                budgetItem.CurrentAmount = budgetItem.CurrentAmount - desiredAmount;
            }
            else
            {
                budgetItem.CurrentAmount = budgetItem.CurrentAmount - desiredAmount;
            }

            db.SaveChanges();
        }

    }
}