using AutoMapper;
using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.Service.Dtos;
using BankSystem.Service.Helpers;
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
        private readonly IBaseRepository<int, TransactionHistory> _transactionRepo;
        private readonly IMapper _mapper;

        public AccountService(IBaseRepository<int, Account> accountRepo, 
            IBaseRepository<int, TransactionHistory> transactionRepo, 
            IMapper mapper)
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _mapper = mapper;
        }

        public AccountDto Create(AccountDto entity)
        {
            if (entity == null)
            {
                return null;
            }

            entity.Password = PasswordHelper.HashPassword(entity.Password);
            var result = _accountRepo.Create(_mapper.Map<Account>(entity));
            _accountRepo.CommitChanges();

            return _mapper.Map<AccountDto>(result);
        }

        public int Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public bool IsAccountExisted(int? accountId, string userId, string password = "")
        {
            if (accountId == null || accountId <= 0 || string.IsNullOrEmpty(userId))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(password))
            {
                var passHash = PasswordHelper.HashPassword(password);
                return (_accountRepo.ReadOne(a => a.Id == accountId.Value && a.UserId == userId && a.Password == passHash) != null);
            }

            var result = _accountRepo.ReadOne(a => a.Id == accountId.Value && a.UserId == userId);

            return (result != null);

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

        public IList<TransactionHistoryDto> ReadHistory(string userId, int accountId, int index, int itemPerPage, out int totalItem)
        {
            if (string.IsNullOrEmpty(userId) || accountId < 0)
            {
                totalItem = 0;
                return new List<TransactionHistoryDto>();
            }
            index = index <= 0 ? 1 : index;
            var result = _transactionRepo.Read(a => a.AccountId == accountId && a.Account.UserId == userId)
                                        .OrderByDescending(a => a.CreatedDate)
                                        .Skip((index - 1) * itemPerPage)
                                        .Take(itemPerPage)
                                        .Select(a => new TransactionHistoryDto()
                                        {
                                            AccountId = a.AccountId,
                                            BalanceAtTime = a.BalanceAtTime,
                                            CreatedDate = a.CreatedDate,
                                            InteractionAccountNumber = a.InteractionAccount.AccountNumber,
                                            Type = (TransactionTypeDto)a.Type,
                                            Value = a.Value,
                                            Note = a.Note
                                        }).ToList();

            totalItem = _transactionRepo.Read(a => a.AccountId == accountId && a.Account.UserId == userId).Count();

            return result;
        }

        public AccountDto ReadOneAccountByNumber(string numberAccount)
        {

            if (string.IsNullOrEmpty(numberAccount))
            {
                return null;
            }
            var result = _accountRepo.ReadOne(a => a.AccountNumber == numberAccount);

            return result != null ? _mapper.Map<AccountDto>(result) : null;
        }

        public AccountDto ReadOneById(int id)
        {
            var result = _accountRepo.ReadOne(a => a.Id == id);
            return result == null ? null : _mapper.Map<AccountDto>(result);
        }

        public bool TransferMoney(int accountId, double value, int desAccountId)
        {
            var account = _accountRepo.ReadOne(a => a.Id == accountId);
            var desAccount = _accountRepo.ReadOne(a => a.Id == desAccountId);

            account.Balance -= value;
            desAccount.Balance += value;

            if (account.Balance < 0)
            {
                throw new Exception("Account dont have enough money. Please try again.");
            }

            _accountRepo.Update(account);
            _accountRepo.Update(desAccount);

            var completed = (_accountRepo.CommitChanges() > 0);

            if (completed)
            {
                _transactionRepo.Create(new TransactionHistory()
                {
                    AccountId = accountId,
                    InteractionAccountId = desAccountId,
                    Type = TransactionType.FundTransfer,
                    Value = value,
                    BalanceAtTime = account.Balance
                });

                _transactionRepo.CommitChanges();
            }

            return completed;

        }

        public void Update(AccountDto entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBalance(double value, int accountId)
        {
            var entity = _accountRepo.ReadOne(a => a.Id == accountId);
            entity.Balance += value;

            if (entity.Balance < 0)
            {
                throw new Exception("Account dont have enough money. Please try again.");
            }
            _accountRepo.Update(entity);

            var completed = (_accountRepo.CommitChanges() > 0);

            if (completed)
            {
                _transactionRepo.Create(new TransactionHistory()
                {
                    AccountId = accountId,
                    Type = (value < 0) ? TransactionType.Withdraw : TransactionType.Deposit,
                    Value = value,
                    BalanceAtTime = entity.Balance
                });

                _transactionRepo.CommitChanges();
            }

            return completed;
        }
    }
}
