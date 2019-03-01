using LWFinancial.Enumerations;
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
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Name { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [Required]
        [Range(0.01,100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
        public Decimal Amount { get; set; }
        [Range(0.01, 100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
        public Decimal ReconciledAmount { get; set; }
        public bool Reconciled { get; set; }

        public int AccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public string EnteredById { get; set; }

        public virtual Account Account { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }
        public virtual ApplicationUser EnteredBy { get; set; }
    }
}