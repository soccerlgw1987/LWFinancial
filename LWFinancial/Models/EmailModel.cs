using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Name")]
        public string ToName { get; set; }

        [Required, EmailAddress, Display(Name = "Email")]
        public string ToEmail { get; set; }

        [Required, EmailAddress, Display(Name = "From")]
        public string FromEmail { get; set; }

        [Required, Display(Name = "Subject")]
        [MaxLength(40, ErrorMessage = "Please enter a max length of 40 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Subject { get; set; }

        [Required, Display(Name = "Message")]
        [MaxLength(200, ErrorMessage = "Please enter a max length of 200 characters."), MinLength(3, ErrorMessage = "Please enter a min length of 3 characters.")]
        public string Body { get; set; }
    }
}