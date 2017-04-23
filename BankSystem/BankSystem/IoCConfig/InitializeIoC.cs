using BankSystem.Service.Implementations;
using BankSystem.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.IoCConfig
{
    public class InitializeIoC
    {
        public static void Init(IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}
