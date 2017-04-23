using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.DAL.DomainModels
{
    public class TransactionHistory : BaseEntity<int>
    {
        public TransactionType Type { get; set; }

        public double Money { get; set; }

        public int AccountId { get; set; }
        
        public int DestinationAccountId { get; set; } // for fund transfer.

        public Account Account { get; set; }

        public Account DestinationAccount { get; set; } // for fund transfer.
    }
}
