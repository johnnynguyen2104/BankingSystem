using AutoMapper;
using BankSystem.DAL.DomainModels;
using BankSystem.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Security
{
    public class SecurityMapper : Profile
    {
        public SecurityMapper()
        : this("SecurityProfile")
        {
        }
        protected SecurityMapper(string profileName)
        : base(profileName)
        {
            CreateMap<UserInformation, User>().ReverseMap();
        }
    }
}
