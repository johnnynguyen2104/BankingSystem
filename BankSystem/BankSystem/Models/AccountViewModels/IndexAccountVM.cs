using BankSystem.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Models.AccountViewModels
{
    public class IndexAccountVM
    {
        public bool? TransactionCompleted { get; set; }

        public IList<AccountDto> Accounts { get; set; }
    }
}
