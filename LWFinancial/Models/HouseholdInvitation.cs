using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class HouseholdInvitation
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Please enter a max length of 30 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Title { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Please enter a max length of 100 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Decscription { get; set; }
        [Required]
        public Guid UniqueKey { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime Expires { get; set; }
        public bool Read { get; set; }

        public int HouseholdId { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }

        public virtual Household Household { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}