using BankSystem.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Interfaces
{
    public interface IAccountService : IBaseService<int, AccountDto>
    {

        bool UpdateBalance(double value, int accountId);

        bool IsAccountExisted(int? accountId, string userId, string password = "");

        /// <summary>
        /// Returning list of accounts
        /// </summary>
        /// <returns>Returning list of accounts</returns>
        IList<AccountDto> ReadAccount(string userId);

        IList<TransactionHistoryDto> ReadHistory(string userId, int accountId, int index, int itemPerPage, out int totalItem);
    }
}
