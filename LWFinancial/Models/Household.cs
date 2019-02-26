using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class Household
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Decscription { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public Household()
        {
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }
    }
}