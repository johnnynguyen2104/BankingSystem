using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.DAL.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BankSystem.Service
{
    public class ModuleConfig
    {
        public static void InitIoC(IServiceCollection services)
        {
            services.AddTransient<IBaseRepository<int, Account>, Repository<int, Account>>();
        }
    }
}
