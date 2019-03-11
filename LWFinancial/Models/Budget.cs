using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class Budget
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Please enter a max length of 100 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Decscription { get; set; }
        [Required]
        [Range(0.01, 100000, ErrorMessage = "Please enter an amount between 0.01 and 100,000.")]
        public Decimal DesiredAmount { get; set; }
        public Decimal? CurrentAmount { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public Budget()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }
    }
}