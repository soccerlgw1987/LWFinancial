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

        public ICollection<BudgetItem> ListBudgetItems(int householdId)
        {
            var household = db.Households.Find(householdId);

            var budgetsItems = household.Budgets.SelectMany(b => b.BudgetItems);

            //var budgetsItems = db.Households.Find(householdId).Budgets.SelectMany(b => b.BudgetItems);

            return budgetsItems.ToList();
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