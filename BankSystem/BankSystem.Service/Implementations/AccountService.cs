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
            if (entity == null 
                || (string.IsNullOrEmpty(entity.Password) || entity.Password.Trim().Length == 0)
                || (string.IsNullOrEmpty(entity.AccountName) || entity.AccountName.Trim().Length == 0)
                || (string.IsNullOrEmpty(entity.AccountNumber) || entity.AccountNumber.Trim().Length == 0))
            {
                return null;
            }

            entity.Password = PasswordHelper.HashPassword(entity.Password);
            var result = _accountRepo.Create(_mapper.Map<Account>(entity));
            

            return _accountRepo.CommitChanges() > 0 ? _mapper.Map<AccountDto>(result) : null;
        }

        public int Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public bool IsAccountExisted(int? accountId, string userId, string password = "")
        {
            if (accountId == null || accountId <= 0 || (string.IsNullOrEmpty(userId) || userId.Trim().Length == 0))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(password))
            {
                var passHash = PasswordHelper.HashPassword(password);
                return (_accountRepo.Read(a => a.Id == accountId.Value && a.UserId == userId && a.Password == passHash).Count() > 0);
            }

            var result = _accountRepo.Read(a => a.Id == accountId.Value && a.UserId == userId).Count();

            return (result > 0);

        }

        public IList<AccountDto> ReadAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId) || userId.Trim().Length == 0)
            {
                return new List<AccountDto>();
            }
            var data = _accountRepo.Read(a => a.UserId == userId).ToList();

            return _mapper.Map<IList<AccountDto>>(data);
        }

        public IList<TransactionHistoryDto> ReadHistory(string userId, int accountId, int index, int itemPerPage, out int totalItem)
        {
            if ((string.IsNullOrEmpty(userId) || userId.Trim().Length == 0) || accountId < 0)
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

            if (string.IsNullOrEmpty(numberAccount) || numberAccount.Trim().Length == 0)
            {
                return null;
            }
            var result = _accountRepo.ReadOne(a => a.AccountNumber == numberAccount);

            return result != null ? _mapper.Map<AccountDto>(result) : null;
        }

        public AccountDto ReadOneById(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var result = _accountRepo.ReadOne(a => a.Id == id);
            return result == null ? null : _mapper.Map<AccountDto>(result);
        }

        public bool TransferMoney(int accountId, double value, int desAccountId)
        {
            if (value <= 0)
            {
                return false;
            }
            var account = _accountRepo.ReadOne(a => a.Id == accountId);
            var desAccount = _accountRepo.ReadOne(a => a.Id == desAccountId);

            if (account != null && desAccount != null)
            {
                if (account.Balance < value)
                {
                    throw new Exception("Account dont have enough money. Please try again.");
                }

                account.Balance -= value;
                desAccount.Balance += value;               

                _accountRepo.Update(account);
                _accountRepo.Update(desAccount);

                var completed = (_accountRepo.CommitChanges() > 0);

                if (completed)
                {
                    var isSuccess = CreateTransactionHistoryFundTransfer(value, account, desAccount);
                    if (isSuccess <= 0)
                    {
                        throw new Exception("Something went wrong. Please try again.");
                    }
                }

                return completed;
            }

            throw new Exception("Something went wrong. Please try again.");
        }

        private int CreateTransactionHistoryFundTransfer(double value, Account account, Account desAccount)
        {
            if (account != null && desAccount != null)
            {
                //transfer
                _transactionRepo.Create(new TransactionHistory()
                {
                    AccountId = account.Id,
                    InteractionAccountId = desAccount.Id,
                    Type = TransactionType.FundTransfer,
                    Value = value,
                    BalanceAtTime = account.Balance
                });

                //received
                _transactionRepo.Create(new TransactionHistory()
                {
                    AccountId = desAccount.Id,
                    InteractionAccountId = account.Id,
                    Type = TransactionType.Received,
                    Value = value,
                    BalanceAtTime = desAccount.Balance
                });

                return _transactionRepo.CommitChanges();
            }
            throw new Exception("Something went wrong. Please try again.");
        }

        public void Update(AccountDto entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBalance(double value, int accountId, string userId)
        {
            if (value == 0 || accountId <= 0)
            {
                return false;
            }

            var entity = _accountRepo.ReadOne(a => a.Id == accountId && a.UserId == userId);
            if (entity != null)
            {
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

            throw new Exception("Invalid account."); ;
        }
    }
}
