using BankSystem.Service.Dtos;
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
                    new object[] { new AccountDto() { AccountName = "ABC_2", UserId = "1", AccountNumber = "123-2", Balance = 1000, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_3", UserId = "2", AccountNumber = "123-3", Balance = 2000, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_4", UserId = "2", AccountNumber = "123-4", Balance = 10, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_5", UserId = "3", AccountNumber = "123-5", Balance = 200, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_6", UserId = "4", AccountNumber = "123-6", Balance = 3000, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_7", UserId = "5", AccountNumber = "123-7", Balance = 100, Password = "123456" } }
        };

        public static object[] CreationAccount_Null = new object[]
        {
                    new object[] { null },
                    new object[] { new AccountDto() { AccountName = "", UserId = "1", AccountNumber = "123-2", Balance = 1000, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = null, UserId = "2", AccountNumber = "123-3", Balance = 2000, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_4", UserId = "2", AccountNumber = "", Balance = 10, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_5", UserId = "3", AccountNumber = null, Balance = 200, Password = "123456" } },
                    new object[] { new AccountDto() { AccountName = "ABC_6", UserId = "4", AccountNumber = "123-6", Balance = 3000, Password = "" } },
                    new object[] { new AccountDto() { AccountName = "ABC_7", UserId = "5", AccountNumber = "123-7", Balance = 100, Password = null } },
                    new object[] { new AccountDto() { AccountName = null, UserId = "5", AccountNumber = null, Balance = 100, Password = null } },
                    new object[] { new AccountDto() { AccountName = "", UserId = "5", AccountNumber = "", Balance = 100, Password = "" } }
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

        public static object[] UpdateBalance_Fail = new object[]
       {
            new object[] { -1, 0, "1" },
            new object[] { -2, -100, "1" },
            new object[] { 0, 1000, "1" },
            new object[] { -3, -10, "3" },
            new object[] { -10, 0, "2" },
            new object[] { -100, -1000, "1" },
            new object[] { -4, 1, "2" }
       };

        public static object[] UpdateBalance_Throw = new object[]
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

        public static object[] Transfer_True = new object[]
       {
                    new object[] { 100, 10, 222 },
                    new object[] { 223, 30, 1 },
                    new object[] { 300, 40, 3 },
                    new object[] { 5, 555, 33 },
                    new object[] { 5, 19, 21 },
                    new object[] { 100, 52, 44 },
                    new object[] { 4, 5, 1 }
       };


        public static object[] ReadAccountByNumber_Valid = new object[]
         {
                        new object[] { "Hello" },
                        new object[] { "ABC" },
                        new object[] { "DSD" },
                        new object[] { "AAS" }
         };

        public static object[] ReadAccountByNumber_Invalid = new object[]
        {
                        new object[] { "" },
                        new object[] { "   " },
                        new object[] { null }
        };

        public static object[] CheckAccountExisted_Existed = new object[]
        {
             new object[] { 1, 2 },
                        new object[] { 2, 3, "asda" },
                        new object[] { 3, 4, "" },
                        new object[] { 1, 3, null }
        };

        public static object[] CheckAccountExisted_Unexisted = new object[]
          {
                 new object[] { 1, null },
                            new object[] { 2, null, "" },
                            new object[] { -1, "" },
                            new object[] { -1, null, null }
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
                            new object[] { "3", 2, -4, 10 },
                            new object[] { "4", 3, -3, 4 }
          };
    }
}
