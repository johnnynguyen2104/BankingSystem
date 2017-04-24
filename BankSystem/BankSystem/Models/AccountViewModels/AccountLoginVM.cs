using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class AccountLoginVM
    {
        public int? AccountId { get; set; }

        public string Password { get; set; }
    }
}
