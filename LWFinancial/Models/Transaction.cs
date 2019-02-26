using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public decimal Amount { get; set; }
        public bool Reconciled { get; set; }
        public decimal ReconciledAmount { get; set; }

        public int AccountId { get; set; }
        public int BudgetItemId { get; set; }
        public int EnteredById { get; set; }

        public virtual Account Account { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }
        public virtual ApplicationUser EnteredBy { get; set; }
    }
}