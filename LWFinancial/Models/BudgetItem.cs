using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
        public Decimal DesiredAmount { get; set; }
        public Decimal? CurrentAmount { get; set; }

        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}