using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class SideBarVM
    {
        public int AccountId { get; set; }

        public bool IsTransactionHistory { get; set; }

        public bool IsWithdraw { get; set; }

        public bool IsDeposit { get; set; }

        public bool IsFundTransfer { get; set; }

        public bool IsBalanceChecking { get; set; }
    }
}
