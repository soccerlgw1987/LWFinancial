using LWFinancial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Helpers
{
    public class BudgetHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Budget> ListHouseholdBudgets(int householdId)
        {
            Household house = db.Households.Find(householdId);

            var budgets = house.Budgets.ToList();

            return budgets;
        }

        public void UpdateBudgetIncome(int budgetId, decimal desiredAmount)
        {
            Budget budget = db.Budgets.Find(budgetId);
            if (budget.CurrentAmount == null)
            {
                budget.CurrentAmount = 0;
                budget.CurrentAmount = budget.CurrentAmount + desiredAmount;
            }
            else
            {
                budget.CurrentAmount = budget.CurrentAmount + desiredAmount;
            }

            db.SaveChanges();
        }

        public void UpdateBudgetDeleteIncome(int budgetId, decimal desiredAmount)
        {
            Budget budget = db.Budgets.Find(budgetId);
            budget.CurrentAmount = budget.CurrentAmount - desiredAmount;

            db.SaveChanges();
        }
    }
}