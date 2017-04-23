using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.DAL.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using BankSystem.Service.Dtos;
using Microsoft.Extensions.Configuration;
using BankSystem.DAL;

namespace BankSystem.Service
{
    public class ServiceModule
    {
        public static void InitIoC(IServiceCollection services, string connectionString)
        {
            //init ioc module for DAL
            DataModule.InitIoc(services, connectionString);

            services.AddTransient<IBaseRepository<int, Account>, Repository<int, Account>>();
        }
    }

    public class ServiceMapper : Profile
    {
        public ServiceMapper()
            : this("ServiceProfile")
        {
        }
        protected ServiceMapper(string profileName)
        : base(profileName)
        {
            CreateMap<AccountDto, Account>().ReverseMap();
            CreateMap<TransactionHistoryDto, TransactionHistory>().ReverseMap();
            CreateMap<TransactionTypeDto, TransactionType>();
        }
    }
}
