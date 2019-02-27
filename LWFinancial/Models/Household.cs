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
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; }
        [MaxLength(100), MinLength(3)]
        public string Decscription { get; set; }
        public DateTime Created { get; set; }
        public Decimal IncomeAmount { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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