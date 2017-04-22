using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankSystem.DAL.DomainModels
{
    public class User : IdentityUser<string>
    {
        public virtual ICollection<Account> Accounts { get; set; }

        public User()
        {
            this.Id = Guid.NewGuid().ToString();

            // Add any custom User properties/code here
        }
    }
}
