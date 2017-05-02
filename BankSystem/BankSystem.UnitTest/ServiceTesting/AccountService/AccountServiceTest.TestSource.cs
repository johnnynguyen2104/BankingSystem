using BankSystem.Service.Dtos;
using BankSystem.UnitTest.ServiceTesting.TestingModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.UnitTest.ServiceTesting.AccountService
{
    public partial class AccountServiceTest
    {
        public static object[] CreationAccount_Success = new object[]
        {
                    new object[] { new AccountDto() { AccountName= "ABC", UserId = "1", AccountNumber = "123-1", Balance = 0, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_2", UserId = "1", AccountNumber = "123-2", Balance = 1000, Password = "!Iloveyou" } },
                    new object[] { new AccountDto() { AccountName = "ABC_3", UserId = "2", AccountNumber = "123-3", Balance = 2000, Password = "ABc1234" } },
                    new object[] { new AccountDto() { AccountName = "ABC_4", UserId = "2", AccountNumber = "123-4", Balance = 10, Password = "78027" } },
                    new object[] { new AccountDto() { AccountName = "ABC_5", UserId = "3", AccountNumber = "123-5", Balance = 200, Password = "76543" } },
                    new object[] { new AccountDto() { AccountName = "ABC_6", UserId = "4", AccountNumber = "123-6", Balance = 3000, Password = "Abc231" } },
                    new object[] { new AccountDto() { AccountName = "ABC_7", UserId = "5", AccountNumber = "123-7", Balance = 100, Password = "!123456!" } }
        };

        public static object[] CreationAccount_PasswordNullOrEmpty = new object[]
          {
                        new object[] { new AccountDto() { AccountName = "ABC_6", UserId = "4", AccountNumber = "123-6", Balance = 3000, Password = "" } },
                        new object[] { new AccountDto() { AccountName = "ABC_7", UserId = "5", AccountNumber = "123-7", Balance = 100, Password = null } },
                        new object[] { new AccountDto() { AccountName = "ABD", UserId = "5", AccountNumber = "EUR", Balance = 100, Password = "  " } }
          };

        public static object[] CreationAccount_AccountNameNullOrEmpty = new object[]
        {
                    new object[] { new AccountDto() { AccountName = null, UserId = "5", AccountNumber = "aaaaa", Balance = 100, Password = "dadas" } },
                    new object[] { new AccountDto() { AccountName = " ", UserId = "5", AccountNumber = "dasdasd", Balance = 100, Password = "fff" } },
                    new object[] { new AccountDto() { AccountName = "", UserId = "5", AccountNumber = "dasdas", Balance = 100, Password = "aaaaaa" } }
        };

        public static object[] CreationAccount_AccountNumberNullOrEmpty  = new object[]
           {
                        new object[] { new AccountDto() { AccountName = "ABC_4", UserId = "2", AccountNumber = "  ", Balance = 10, Password = "123456" } },
                        new object[] { new AccountDto() { AccountName = "ABC_5", UserId = "3", AccountNumber = null, Balance = 200, Password = "123456" } },
                        new object[] { new AccountDto() { AccountName = "ABC_6", UserId = "4", AccountNumber = "", Balance = 3000, Password = "dadasd" } },
           };

        public static object[] UpdateBalance_Success = new object[]
        {
            new object[] { 1, 1000, "1" },
            new object[] { 1, -100, "1" },
            new object[] { 2, 1000, "1" },
            new object[] { 5, -10, "3" },
            new object[] { 4, 1000, "2" },
            new object[] { 2, -1000, "1" },
            new object[] { 3, 1, "2" }
        };

        public static object[] UpdateBalance_InvalidAccountId = new object[]
       {
            new object[] { -1, 3, "1" },
            new object[] { -2, -100, "1" },
            new object[] { 0, 1000, "1" },
            new object[] { -3, -10, "3" },
            new object[] { -10, 5, "2" },
            new object[] { -100, -1000, "1" },
            new object[] { -4, 1, "2" }
       };

        public static object[] UpdateBalance_InvalidUserId = new object[]
      {
            new object[] { 2, -100, "" },
            new object[] { 2, 1000, " " },
            new object[] { 3, -10, null },

      };

        public static object[] UpdateBalance_AccountNotExsisted = new object[]
         {
                new object[] { 100, 55, "222" },
                new object[] { 2, -100, "55" },
                new object[] { 300, 1000, "22" },
                new object[] { 5, -10, "33" },
                new object[] { 5, 2, "21" },
                new object[] { 100, -1000, "-1" },
                new object[] { 4, 1, "-23" }
         };

        public static object[] Transfer_Throw = new object[]
       {
                new object[] { 100, 55, 222 },
                new object[] { 223, 100, 1 },
                new object[] { 300, 1000, 3 },
                new object[] { 5, 10, 33 },
                new object[] { 5, 2, 21 },
                new object[] { 100, 1000, 44 },
                new object[] { 4, 1, 55 }
       };

        public static object[] Transfer_False = new object[]
      {
                new object[] { 100, 0, 222 },
                new object[] { 223, 0, 1 },
                new object[] { 300, 0, 3 },
                new object[] { 5, 0, 33 },
                new object[] { 5, -1999, 21 },
                new object[] { 100, -1000, 44 },
                new object[] { 4, -1, -1 }
      };

        public static object[] Transfer_Success = new object[]
        {
            new object[] { 1, 55, 2 },
            new object[] { 2, 5, 3 },
            new object[] { 2, 5.25, 3 },
            new object[] { 2, 10.03, 3 },
            new object[] { 8, 102.52, 2 },
            new object[] { 1, 5, 2 },
            new object[] { 2, 10, 3 },
            new object[] { 3, 0.25, 2 },
            new object[] { 2, 3.25, 3 },
            new object[] { 3, 10.03, 2 },
            new object[] { 2, 10.23, 8 }
        };

        public static object[] Transfer_Success_Concurrency = new object[]
       {
            new object[]{1, 55, 2 },
            new object[]{2, 5, 3 },
            new object[]{ 2, 5.25, 3 },
            new object[]{ 2, 10.03, 3 },
            new object[]{ 8, 102.52, 2 },
            new object[]{1, 5, 2 },
            new object[]{2, 10, 3 },
            new object[]{ 3, 0.25, 2 },
            new object[]{2, 3.25, 3 },
            new object[]{3, 10.03, 2 },
            new object[]{2, 10.23, 8 }
       };


        public static object[] ReadAccountByNumber_Valid = new object[]
         {
                        new object[] { "123-1" },
                        new object[] { "123-2" },
                        new object[] { "123-4" },
                        new object[] { "123-5" }
         };

        public static object[] ReadAccountByNumber_Invalid = new object[]
        {
                        new object[] { "" },
                        new object[] { "   " },
                        new object[] { null }
        };

        public static object[] ReadAccountByNumber_NotExisted = new object[]
       {
                        new object[] { "12" },
                        new object[] { "32" },
                        new object[] { "dsd-12" }
       };

        public static object[] CheckAccountExisted_ExistedNoPassword = new object[]
        {
             new object[] { 1, 1 },
             new object[] { 2, 1 },
             new object[] { 3, 2 },
             new object[] { 5, 3 }
        };

        public static object[] CheckAccountExisted_ExistedWithPassword = new object[]
       {
             new object[] { 1, 1, "123456" },
             new object[] { 2, 1, "123456" },
             new object[] { 3, 2, "123456" },
             new object[] { 5, 3, "123456" }
       };

        public static object[] CheckAccountExisted_Unexisted = new object[]
          {
                 new object[] { 34, "3" },
                            new object[] { 222, "1" },
                            new object[] {44, "1" }
          };

        public static object[] ReadAccount_Valid = new object[]
       {
                 new object[] { "1" },
                            new object[] { "2" },
                            new object[] { "-1" },
                            new object[] { "1231231" }
       };

        public static object[] ReadAccount_Invalid = new object[]
       {
                 new object[] { "" },
                            new object[] { "    " },
                            new object[] { null }
       };

        public static object[] ReadTransactions_Invalid = new object[]
          {
                            new object[] { "", 1, 1, -1 },
                            new object[] { "   ", -2, -1, -4 },
                            new object[] { null, 1, 1, 1 },
                             new object[] { "2", 1, 1, -10 },
                             new object[] { "2", 1, 1, 0 }
          };

        public static object[] ReadTransactions_Valid = new object[]
          {
                            new object[] { "1", 1, 1, 1 },
                            new object[] { "1", 2, 4, 10 },
                            new object[] { "2", 3, 3, 4 }
          };
    }
}
