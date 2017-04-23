using BankSystem.DAL.DomainModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankSystem.DAL
{
    public class DataModule
    {
        public static void InitIoc(IServiceCollection services, string connectionString)
        {
            // Add framework services.
            services.AddDbContext<BankSystemDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
