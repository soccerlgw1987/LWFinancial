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
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; }
        public Decimal InitialBalance { get; set; }
        public Decimal CurrentBalance { get; set; }
        public Decimal ReconciledBalance { get; set; }
        public Decimal LowBalanceWarning { get; set; }
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