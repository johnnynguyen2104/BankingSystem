using BankSystem.DAL.DomainModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using BankSystem.DAL.Interfaces;

namespace BankSystem.DAL
{
    public class DataModule
    {
        public static void InitIoc(IServiceCollection services, string connectionString)
        {
            // Add framework services.
            services.AddDbContext<BankSystemDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<IDbContext, BankSystemDbContext>();
        }
    }
}
