using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LWFinancial.Models
{
    public class AccountEdit
    {
        public Account Accounts { get; set; }

        public AccountEdit()
        {
            this.Accounts = new Account();
        }
    }
}