using AutoMapper;
using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Implementations;
using BankSystem.DAL.Interfaces;
using BankSystem.Service;
using BankSystem.Service.Dtos;
using BankSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BankSystem.UnitTest.ServiceTesting.AccountService
{
    public partial class SetupAccountServiceTest
    {
        public Mock<IBaseRepository<int, Account>> _accountRepoMock;
        public Mock<IBaseRepository<int, TransactionHistory>> _transactionRepoMock;
        public IMapper _mapper;

        public IAccountService _accountService;

        public SetupAccountServiceTest()
        {
            _mapper = IntiMapper();
            _accountRepoMock = new Mock<IBaseRepository<int, Account>>();
            _transactionRepoMock = new Mock<IBaseRepository<int, TransactionHistory>>();

            _accountService = new Service.Implementations.AccountService(_accountRepoMock.Object, _transactionRepoMock.Object, _mapper);
        }

        public IMapper IntiMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceMapper());
            });

            return config.CreateMapper();
        }

        public static IQueryable<Account> AccountFakeDb =
             new List<Account>()
            {
                new Account(){ Id = 1, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC", UserId = "1", AccountNumber = "123-1", Balance = 1000, Password= "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" },
                new Account(){ Id = 2, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_2", UserId = "1", AccountNumber = "123-2", Balance = 1000, Password= "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" },
                new Account(){ Id = 3, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_3", UserId = "2", AccountNumber = "123-3", Balance = 2000, Password= "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" },
                new Account(){ Id = 4, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_4", UserId = "2", AccountNumber = "123-4", Balance = 1000, Password= "1235" },
                new Account(){ Id = 5, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_5", UserId = "3", AccountNumber = "123-5", Balance = 2000, Password= "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" },
                new Account(){ Id = 6, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_6", UserId = "4", AccountNumber = "123-6", Balance = 3000, Password= "12356" },
                new Account(){ Id = 7, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_7", UserId = "5", AccountNumber = "123-7", Balance = 10000, Password= "12345" },
                 new Account(){ Id = 8, RowVersion = BitConverter.GetBytes(DateTime.Now.Ticks), AccountName= "ABC_8", UserId = "5", AccountNumber = "123-8", Balance = 213.43, Password= "12345" }
            }.AsQueryable();



        public static IQueryable<TransactionHistory> TransactionFakeDb =
            new List<TransactionHistory>()
            {
                new TransactionHistory(){ Id = 1, Note= "ABC", AccountId = 1, Account = new Account(){ Id = 1, AccountName= "ABC", UserId = "1" }, Type = TransactionType.Deposit, Value = 100 },
                new TransactionHistory(){ Id = 2, Note= "ABC_2", AccountId = 1, Account = new Account(){ Id = 1, AccountName= "ABC", UserId = "1" }, Type = TransactionType.Received, InteractionAccountId = 2, InteractionAccount = new Account(){ Id = 2, AccountName= "ABC", UserId = "1", AccountNumber = "ABC" } , Value = 1000 },
                new TransactionHistory(){ Id = 3, Note= "ABC_3", AccountId = 2, Account = new Account(){ Id = 2, AccountName= "ABC", UserId = "1" }, Type = TransactionType.Withdraw, Value = 2000 },
                new TransactionHistory(){ Id = 4, Note= "ABC_4", AccountId = 2, Account = new Account(){ Id = 2, AccountName= "ABC", UserId = "1" }, Type = TransactionType.Deposit, Value = 10 },
                new TransactionHistory(){ Id = 5, Note= "ABC_5", AccountId = 3, Account = new Account(){ Id = 3, AccountName= "ABC", UserId = "2" }, Type = TransactionType.FundTransfer, InteractionAccountId = 1, InteractionAccount = new Account(){ Id = 1, AccountName= "ABC", UserId = "1", AccountNumber = "ABC-1" }, Value = 200 },
                new TransactionHistory(){ Id = 6, Note= "ABC_6", AccountId = 4, Account = new Account(){ Id = 4, AccountName= "ABC", UserId = "2" }, Type = TransactionType.Deposit, Value = 3000 },
                new TransactionHistory(){ Id = 7, Note= "ABC_7", AccountId = 5, Account = new Account(){ Id = 5, AccountName= "ABC", UserId = "3" }, Type = TransactionType.Withdraw, Value = 100 }
            }.AsQueryable();


    }
}
