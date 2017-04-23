using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.DAL.DomainModels
{
    public class TransactionHistory : BaseEntity<int>
    {
        public TransactionType Type { get; set; }

        public double Money { get; set; }

        public double BalanceAtTime { get; set; }

        public int AccountId { get; set; }
        
        public int InteractionAccountId { get; set; } // for fund transfer & receive.

        public string Note { get; set; }

        public Account Account { get; set; }

        public Account InteractionAccount { get; set; } // for fund transfer & receive .
    }
}
