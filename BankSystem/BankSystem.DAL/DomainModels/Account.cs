using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.DAL.DomainModel
{
    public class Account : BaseEntity<int>
    {
        public string AccountName { get; set; }

        public string AccountNumber { get; } = Guid.NewGuid().ToString();

        public double Balance { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
