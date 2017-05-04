using AutoMapper;
using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using BankSystem.Service.Dtos;
using BankSystem.Service.Helpers;
using BankSystem.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="accountRepo">Account repository</param>
        /// <param name="transactionRepo">Transaction History repository</param>
        /// <param name="mapper">Mapper instance</param>
        /// <param name="accountRepo2">This one is for concurrency test</param>
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
            CreationAccountValidator(entity);

            entity.Password = PasswordHelper.HashPassword(entity.Password);
            var result = _accountRepo.Create(_mapper.Map<Account>(entity));


            return _accountRepo.CommitChanges() > 0 ? _mapper.Map<AccountDto>(result) : null;
        }

        private void CreationAccountValidator(AccountDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Argument was null.");
            }

            if ((string.IsNullOrEmpty(entity.Password)
                || entity.Password.Trim().Length == 0))
            {
                throw new ArgumentException("Argument password was null or empty.", "Password");
            }

            if ((string.IsNullOrEmpty(entity.AccountName)
                || entity.AccountName.Trim().Length == 0))
            {
                throw new ArgumentException("Argument AccountName was null or empty.", "AccountName");
            }

            if ((string.IsNullOrEmpty(entity.AccountNumber)
                || entity.AccountNumber.Trim().Length == 0))
            {
                throw new ArgumentException("Argument AccountNumber was null or empty.", "AccountNumber");
            }
        }

        public int Delete(IList<int> ids)
        {
            throw new NotImplementedException();
        }

        public bool IsAccountExisted(int? accountId, string userId, string password = "")
        {
            if (accountId == null 
                || accountId <= 0)
            {
                throw new ArgumentException("Invalid accountId.", "accountId");
            }
            if ((string.IsNullOrEmpty(userId) 
                || userId.Trim().Length == 0))
            {
                throw new ArgumentException("Invalid userId.", "userId");
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
            ReadHistoryValidator(userId, accountId, index, itemPerPage);

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
                                            InteractionAccountNumber = a.InteractionAccount != null ? a.InteractionAccount.AccountNumber : "",
                                            Type = (TransactionTypeDto)a.Type,
                                            Value = a.Value,
                                            Note = a.Note
                                        }).ToList();

            totalItem = _transactionRepo.Read(a => a.AccountId == accountId && a.Account.UserId == userId).Count();

            return result;
        }

        private static void ReadHistoryValidator(string userId, int accountId, int index, int itemPerPage)
        {
            if ((string.IsNullOrEmpty(userId) || userId.Trim().Length == 0))
            {
                throw new ArgumentException("Invalid UserId", "userId");
            }

            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid AccountId", "accountId");
            }

            if (itemPerPage <= 0)
            {
                throw new ArgumentException("Invalid input item per page", "itemPerPage");
            }

            if (index <= 0)
            {

                throw new ArgumentException("Invalid input index.", "index");
            }
        }

        public AccountDto ReadOneAccountByNumber(string numberAccount)
        {

            if (string.IsNullOrEmpty(numberAccount) 
                || numberAccount.Trim().Length == 0)
            {
                throw new ArgumentNullException("numberAccount", "AccountNumber can't be null or empty.");
            }

            var result = _accountRepo.ReadOne(a => a.AccountNumber == numberAccount);

            if (result == null)
            {
                throw new KeyNotFoundException($"Account Nmber {numberAccount} can't be found.");
            }

            return _mapper.Map<AccountDto>(result);
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
                throw new ArgumentException("Invalid value.", "value");
            }

            if (accountId <= 0 || desAccountId <= 0)
            {
                throw new ArgumentException("Invalid source or destination accountId", accountId <= 0 ? "accountId" : "desAccountId");
            }

            if (accountId == desAccountId)
            {
                throw new ArgumentException("Can't transfer to the same account.", "accountId");
            }


            var account = _accountRepo.ReadOne(a => a.Id == accountId);

            if (account != null)
            {
                var desAccount = _accountRepo.ReadOne(a => a.Id == desAccountId);

                if (desAccount == null)
                {
                    throw new KeyNotFoundException("Can't find destination account");
                }

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

            throw new KeyNotFoundException("Can't find source account");
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
            if (value == 0)
            {
                throw new ArgumentException("Value can't be lower than 1", "value");
            }

            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid AccountId.", "AccountId");
            }

            if (string.IsNullOrEmpty(userId) || userId.Trim().Length == 0)
            {
                throw new ArgumentException("Invalid UserId.", "UserId");
            }

            var entity = _accountRepo
                            .ReadOne(a => a.Id == accountId && a.UserId == userId);
            if (entity != null)
            {
                entity.Balance += value;

                if (entity.Balance < 0)
                {
                    throw new ArgumentException("Account dont have enough money. Please try again.", "Balance");
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

            throw new KeyNotFoundException("Account doesn't exist.");
        }
    }
}
