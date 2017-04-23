using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.DAL.DomainModels
{
    public enum TransactionType
    {
        FundTransfer = 0,
        Deposit = 1,
        Withdraw = 2,
        Received = 3
    }
}
