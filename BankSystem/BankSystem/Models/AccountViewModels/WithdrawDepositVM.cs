using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class WithdrawDepositVM
    {
        public double Money { get; set; }

        public int AccountId { get; set; }
    }
}
