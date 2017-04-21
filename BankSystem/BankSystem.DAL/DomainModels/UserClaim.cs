using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BankSystem.DAL.DomainModels
{
    public class UserClaim : IdentityUserClaim<string>
    {
    }
}
