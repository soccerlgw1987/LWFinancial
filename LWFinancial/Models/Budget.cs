using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class Budget
    {
        public int Id { get; set; }
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; }
        [MaxLength(100), MinLength(3)]
        public string Decscription { get; set; }
        public Decimal DesiredAmount { get; set; }
        public Decimal CurrentAmount { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }
    }
}