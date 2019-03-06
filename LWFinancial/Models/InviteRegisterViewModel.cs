using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class InviteRegisterViewModel
    {
        public RegisterViewModel RegisterVM { get; set; }

        [Required]
        public Guid Code { get; set; }

        public InviteRegisterViewModel()
        {
            RegisterVM = new RegisterViewModel();
        }
    }
}