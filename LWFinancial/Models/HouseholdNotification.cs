using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class HouseholdNotification
    {
        public int Id { get; set; }
        [MaxLength(20), MinLength(3)]
        public string Title { get; set; }
        [MaxLength(100), MinLength(3)]
        public string Decscription { get; set; }
        public DateTime Created { get; set; }

        public int HouseholdId { get; set; }
        public string RecipientId { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}