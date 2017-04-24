using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class BalanceCheckingVM
    {
        public int AccountId { get; set; }

        public double Balance { get; set; }
    }
}
