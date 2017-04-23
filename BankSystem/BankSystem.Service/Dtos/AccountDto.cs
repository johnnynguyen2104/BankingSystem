using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Dtos
{
    public class AccountDto : BaseDto<int>
    {
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public double Balance { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }
    }
}
