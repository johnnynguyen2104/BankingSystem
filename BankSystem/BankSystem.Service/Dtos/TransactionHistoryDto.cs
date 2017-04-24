using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Dtos
{
    public class TransactionHistoryDto : BaseDto<int>
    {
        public TransactionTypeDto Type { get; set; }

        public double Value { get; set; }

        public double BalanceAtTime { get; set; }

        public int AccountId { get; set; }

        public int InteractionAccountId { get; set; } // for fund transfer & Receive.

        public string Note { get; set; }

        public string InteractionAccountNumber { get; set; } // for fund transfer & Receive.
    }

    public enum TransactionTypeDto
    {
        FundTransfer = 0,
        Deposit = 1,
        Withdraw = 2,
        Received = 3
    }
}
