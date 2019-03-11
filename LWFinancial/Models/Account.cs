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
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
        public Decimal InitialBalance { get; set; }
        public Decimal? CurrentBalance { get; set; }
        public Decimal? ReconciledBalance { get; set; }
        [Required]
        [Range(0.01, 100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
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