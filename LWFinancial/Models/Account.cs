using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class Account
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal ReconciledBalance { get; set; }
        public DateTime Created { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}