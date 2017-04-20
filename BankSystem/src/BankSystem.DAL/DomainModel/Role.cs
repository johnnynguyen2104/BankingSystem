using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankSystem.DAL.DomainModel
{
    public class Role: IdentityRole<string>
    {
    }
}
