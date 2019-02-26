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
        [MaxLength(20)]
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}