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
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Please enter a max length of 100 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Decscription { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string AvatarPath { get; set; }
        [Required]
        [Range(1, 100000, ErrorMessage = "Please enter an amount between 1 and 100,000.")]
        public Decimal IncomeAmount { get; set; }
        public Decimal? CurrentBudgetAmount { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<HouseholdNotification> HouseholdNotifications { get; set; }
        public virtual ICollection<HouseholdInvitation> HouseholdInvitations { get; set; }

        public Household()
        {
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.ApplicationUsers = new HashSet<ApplicationUser>();
            this.HouseholdNotifications = new HashSet<HouseholdNotification>();
            this.HouseholdInvitations = new HashSet<HouseholdInvitation>();
        }
    }  
}