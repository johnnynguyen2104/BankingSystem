using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Dtos
{
    public class TransactionHistoryDto
    {
        public TransactionTypeDto Type { get; set; }

        public double Money { get; set; }

        public int AccountId { get; set; }

        public int DestinationAccountId { get; set; } // for fund transfer.
    }

    public enum TransactionTypeDto
    {
        FundTransfer = 0,
        Deposit = 1,
        Withdraw = 2
    }
}
