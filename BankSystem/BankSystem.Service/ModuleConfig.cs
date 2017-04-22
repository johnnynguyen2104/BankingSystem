using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.DAL.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using BankSystem.Service.Dtos;

namespace BankSystem.Service
{
    public class ModuleConfig
    {
        public static void InitIoC(IServiceCollection services)
        {
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
        }
    }
}
