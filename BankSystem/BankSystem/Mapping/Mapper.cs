using AutoMapper;
using BankSystem.Models.AccountViewModels;
using BankSystem.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Mapping
{
    public class ViewModelMapper : Profile
    {
        public ViewModelMapper()
            : this("ViewModelProfile")
        {
        }
        protected ViewModelMapper(string profileName)
        : base(profileName)
        {
            CreateMap<AccountDto, AccountCreationVM>().ReverseMap();
        }
    }
}
