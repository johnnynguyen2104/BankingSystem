using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.Service.Dtos;
using BankSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Implementations
{
    public class AccountService : IBaseService<int, AccountDto>
    {
        private readonly IBaseRepository<int, Account> _accountRepo;

        public AccountService(IBaseRepository<int, Account> accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public AccountDto Create(AccountDto entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public IList<AccountDto> Read()
        {
            throw new NotImplementedException();
        }

        public AccountDto ReadOneById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(AccountDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
