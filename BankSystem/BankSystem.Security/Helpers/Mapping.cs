using BankSystem.DAL.DomainModels;
using BankSystem.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Security.Helpers
{
    public static class Mapping
    {
        public static User MapFromUserInfo(this UserInformation userInfo)
        {
            if (userInfo == null)
            {
                return null;
            }

            return new User()
            {
                Id = userInfo.Id,
                Email = userInfo.Email,
                PhoneNumber = userInfo.PhoneNumber,
                PasswordHash = userInfo.Password,
                UserName = userInfo.UserName
            };
        }

        public static UserInformation MapFromUserInfo(this User user)
        {

            if (user == null)
            {
                return null;
            }

            return new UserInformation()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.PasswordHash,
                UserName = user.UserName
            };
        }
    }
}
