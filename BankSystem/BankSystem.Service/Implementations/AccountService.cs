using AutoMapper;
using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.Service.Dtos;
using BankSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankSystem.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseRepository<int, Account> _accountRepo;
        private readonly IMapper _mapper;

        public AccountService(IBaseRepository<int, Account> accountRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public AccountDto Create(AccountDto entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = _accountRepo.Create(_mapper.Map<Account>(entity));
            _accountRepo.CommitChanges();

            return _mapper.Map<AccountDto>(result);
        }

        public int Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public IList<AccountDto> ReadAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new List<AccountDto>();
            }
            var data = _accountRepo.Read(a => a.UserId == userId).ToList();

            return _mapper.Map<IList<AccountDto>>(data);
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
