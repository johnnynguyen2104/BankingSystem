using BankSystem.DAL.DomainModels;
using BankSystem.Service.Dtos;
using BankSystem.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace BankSystem.UnitTest.ServiceTesting.AccountService
{
 
    public partial class AccountServiceTest
    {

        #region Creation of Account
        [Theory]
        [MemberData(nameof(CreationAccount_Success))]
        public void GivenValidAccountInfo_WhenCreate_AccountIdGreaterThanZeroAndHashedPassword(AccountDto entity)
        {
            //arrange 
            string originalPass = entity.Password;

            _accountRepoMock.Setup(x => x.Create(It.IsAny<Account>()))
                .Returns(delegate() 
                {
                    entity.Id = 1;
                    return _mapper.Map<Account>(entity);
                });
            _accountRepoMock.Setup(x => x.CommitChanges())
                .Returns(1);
            //Action
            var result = _accountService.Create(entity);

            //Assert
            Assert.True(result.Id > 0);
            Assert.True(result.Password != originalPass);
        }

        [Fact]
        public void GivenNullArgument_WhenCreate_ReturnArgumentNullException()
        {
            //arrange 
            ArgumentNullException result;
            //Action
            result = Assert.Throws<ArgumentNullException>(() => _accountService.Create(null));
            //assert
            Assert.True(result.ParamName == "entity");
        }

        [Theory]
        [MemberData(nameof(CreationAccount_PasswordNullOrEmpty))]
        public void GivenNullOrEmmty_Password_WhenCreate_ReturnArgumentException(AccountDto entity)
        {
            //arrange 
            ArgumentException result;
            //Action
            result = Assert.Throws<ArgumentException>(() => _accountService.Create(entity));
            //assert
            Assert.True(result.ParamName == "Password");
        }

        [Theory]
        [MemberData(nameof(CreationAccount_AccountNameNullOrEmpty))]
        public void GivenNullOrEmmty_AccountName_WhenCreate_ReturnArgumentException(AccountDto entity)
        {
            //arrange 
            ArgumentException result;
            //Action
            result = Assert.Throws<ArgumentException>(() => _accountService.Create(entity));
            //assert
            Assert.True(result.ParamName == "AccountName");
        }

        [Theory]
        [MemberData(nameof(CreationAccount_AccountNumberNullOrEmpty))]
        public void GivenNullOrEmmty_AccountNumber_WhenCreate_ReturnArgumentException(AccountDto entity)
        {
            //arrange 
            ArgumentException result;
            //Action
            result = Assert.Throws<ArgumentException>(() => _accountService.Create(entity));
            //assert
            Assert.True(result.ParamName == "AccountNumber");
        }
        #endregion

        #region Withdraw and Deposit 

        [Theory]
        [MemberData(nameof(UpdateBalance_Success))]
        public void GivenValidData_WhenUpdateBalance_Success(int accountId, double value, string userId)
        {
            //arrange 
            Account account = new Account() { Id = accountId, AccountName = "A", AccountNumber = "ABC-1", Balance = 10000, UserId = userId };
            double originalValue = account.Balance;
            Expression<Func<Account, bool>> expression = (a => a.Id == accountId && a.UserId == userId);

            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(account)
                .Verifiable();
            _accountRepoMock.Setup(a => a.CommitChanges())
                .Returns(1);

            _transactionRepoMock.Setup(a => a.Create(It.IsAny<TransactionHistory>()))
                .Verifiable("Creation of transaction should be call at least once");
            _transactionRepoMock.Setup(a => a.CommitChanges())
                .Verifiable("Saving changes transaction should be call at least once");
            // Action
            var result = _accountService.UpdateBalance(value, accountId, userId);

            //assert
            Assert.True((account.Balance == (originalValue + value)));
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
            _transactionRepoMock.Verify(a => a.Create(It.IsAny<TransactionHistory>()), Times.Once);
            _transactionRepoMock.Verify(a => a.CommitChanges(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_InvalidAccountId))]
        public void GivenInvalidAccountId_WhenUpdateBalance_ThrowArgumentException(int accountId, double value, string userId)
        {
            //arrange & Action
            ArgumentException exceptionResult 
                = Assert.Throws<ArgumentException>(() => _accountService.UpdateBalance(value, accountId, userId));

            //assert
            Assert.Equal("AccountId", exceptionResult.ParamName);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_InvalidUserId))]
        public void GivenInValidUserId_WhenUpdateBalance_ThrowArgumentException(int accountId, double value, string userId)
        {
            //arrange & Action
            ArgumentException exceptionResult
                = Assert.Throws<ArgumentException>(() => _accountService.UpdateBalance(value, accountId, userId));

            //assert
            Assert.Equal("UserId", exceptionResult.ParamName);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_AccountNotExsisted))]
        public void GivenNotExsistedAccount_User_WhenUpdateBalance_ThrowException(int accountId, double value, string userId)
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>())).Verifiable();
            KeyNotFoundException ex = Assert.Throws<KeyNotFoundException>(() => _accountService.UpdateBalance(value, accountId, userId));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
            _accountRepoMock.Verify(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
        }

        [Fact]
        public void GivenAccountNotEnoughMoney_WhenWithdraw_ThrowException()
        {
            //arrange 
            Account accountForTest = new Account() { Id = 1, UserId = "1", Balance = 0 };
            //Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>())).Returns(accountForTest).Verifiable();
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _accountService.UpdateBalance(-100, 1, "1"));

            //assert
            Assert.Equal("Balance", ex.ParamName);
            _accountRepoMock.Verify(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
        }
        #endregion

        #region Transfer
        [Fact]
        public void GivenInvalidValue_WhenTransfer_ThrowArgumentException()
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Verifiable("ReadOne shouldn't be called because invalid value.");
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _accountService.TransferMoney(1,  -1, 2));

            //assert
            Assert.True(ex.ParamName == "value");
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Never);
        }

        [Theory]
        [InlineData(new object[] { -1, 55, 222 })]
        [InlineData(new object[] { 1, 55, -222 })]
        public void GivenInvalidSource_DesAccountId_WhenTransfer_ThrowArgumentException(int accountId, double value, int desAccountId)
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Verifiable("ReadOne shouldn't be called because invalid value.");
            _accountRepoMock.Setup(x => x.CommitChanges()).Verifiable();
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _accountService.TransferMoney(accountId, value, desAccountId));

            //assert
            Assert.True(ex.ParamName == "accountId" || ex.ParamName == "desAccountId");
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Never);
            _accountRepoMock.Verify(a => a.CommitChanges(), Times.Never);
        }

        [Fact]
        public void GivenSameSouce_DesAccountId_WhenTransfer_ThrowArgumentException()
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Verifiable("ReadOne shouldn't be called because invalid value.");
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _accountService.TransferMoney(1, 10, 1));

            //assert
            Assert.True(ex.ParamName == "accountId");
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Never);
        }

        [Fact]
        public void GivenNotEnoughMoney_WhenTransfer_ThrowException()
        {
            //arrange
            Account account = new Account() { Id = 1, UserId = "1", Balance = 0 };
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(account)
                .Verifiable();
            _accountRepoMock.Setup(x => x.CommitChanges()).Verifiable();

            _transactionRepoMock.Setup(a => a.Create(It.IsAny<TransactionHistory>())).Verifiable();

            //action
            Exception ex = Assert.Throws<Exception>(() => _accountService.TransferMoney(1, 1000, 2));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
            Assert.Equal(0, account.Balance);
            _accountRepoMock.Verify(a => a.CommitChanges(), Times.Never);
            _transactionRepoMock.Verify(a => a.Create(It.IsAny<TransactionHistory>()), Times.Never);
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
        }


        [Fact]
        public void GivenNotExistedAccountId_WhenTransfer_ThrowKeyNotFoundException()
        {
            //arrange
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Verifiable();

            _accountRepoMock.Setup(x => x.CommitChanges()).Verifiable();

            _transactionRepoMock.Setup(a => a.Create(It.IsAny<TransactionHistory>())).Verifiable();

            //action
            KeyNotFoundException ex = Assert.Throws<KeyNotFoundException>(() => _accountService.TransferMoney(1, 1000, 2));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
            _accountRepoMock.Verify(a => a.CommitChanges(), Times.Never);
            _transactionRepoMock.Verify(a => a.Create(It.IsAny<TransactionHistory>()), Times.Never);
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Once);
        }

        [Fact]
        public void GivenNotExistedDestinationAccountId_WhenTransfer_ThrowKeyNotFoundException()
        {
            //arrange
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns((Expression<Func<Account, bool>> expression) => {
                    var result = AccountFakeDb.SingleOrDefault(expression);
                    return result;
                })
                .Verifiable();

            _accountRepoMock.Setup(x => x.CommitChanges()).Verifiable();

            _transactionRepoMock.Setup(a => a.Create(It.IsAny<TransactionHistory>())).Verifiable();

            //action
            KeyNotFoundException ex = Assert.Throws<KeyNotFoundException>(() => _accountService.TransferMoney(1, 1000, 200));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
            _accountRepoMock.Verify(a => a.CommitChanges(), Times.Never);
            _transactionRepoMock.Verify(a => a.Create(It.IsAny<TransactionHistory>()), Times.Never);
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Exactly(2));
        }

        [Theory]
        [InlineData(new object[] { 1, 55, 2 })]
        [InlineData(new object[] { 2, 5, 3 })]
        public void GivenCorrectData_WhenTransfer_Success(int accountId, double value, int desAccountId)
        {
            //arrange
            var sourceAccount = AccountFakeDb.SingleOrDefault(a => a.Id == accountId);
            var desAccount = AccountFakeDb.SingleOrDefault(a => a.Id == desAccountId);
            double originalSAccountBalance = sourceAccount.Balance
                , originalDAccountBalance = desAccount.Balance; 

            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns((Expression<Func<Account, bool>> expression) => {
                    var data = AccountFakeDb.SingleOrDefault(expression);
                    return data;
                })
                .Verifiable();

            _accountRepoMock.Setup(x => x.CommitChanges()).Returns(1).Verifiable();

            _transactionRepoMock.Setup(a => a.Create(It.IsAny<TransactionHistory>())).Verifiable();
            _transactionRepoMock.Setup(a => a.CommitChanges()).Returns(1).Verifiable();
            //action
            var result = _accountService.TransferMoney(accountId, value, desAccountId);

            //assert
            Assert.True(sourceAccount.Balance == (originalSAccountBalance - value));
            Assert.True(desAccount.Balance == (originalDAccountBalance + value));
            _accountRepoMock.Verify(a => a.CommitChanges(), Times.Once);
            _accountRepoMock.Verify(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()), Times.Exactly(2));
            _transactionRepoMock.Verify(a => a.Create(It.IsAny<TransactionHistory>()), Times.Exactly(2));
            _transactionRepoMock.Verify(a => a.CommitChanges(), Times.Once);
        }
        #endregion

        #region Read accounts by number
        [Theory]
        [MemberData(nameof(ReadAccountByNumber_Valid))]
        public void GivenValidData_WhenReadAccountByNumber_Account(string accountNumber)
        {
            //arrange
            _accountRepoMock.Setup(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns((Expression<Func<Account, bool>> expression) => {
                    var data = AccountFakeDb.SingleOrDefault(expression);
                    return data;
                });
            //Action
            var result = _accountService.ReadOneAccountByNumber(accountNumber);

            //assert
            Assert.NotNull(result);
            Assert.True(result.AccountNumber == accountNumber);
        }

        [Theory]
        [MemberData(nameof(ReadAccountByNumber_Invalid))]
        public void GivenEmptyOrNullAccountNumber_WhenReadAccountByNumber_ThrowArgumentNullException(string accountNumber)
        {
            //arrange & Action
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => _accountService.ReadOneAccountByNumber(accountNumber));

            //assert
            Assert.True(result.ParamName == "numberAccount");
        }

        [Theory]
        [MemberData(nameof(ReadAccountByNumber_NotExisted))]
        public void GivenAccountNotExisted_WhenReadAccountByNumber_ThrowArgumentNullException(string accountNumber)
        {
            //arrange 
            _accountRepoMock.Setup(a => a.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()))
               .Returns((Expression<Func<Account, bool>> expression) => {
                   var data = AccountFakeDb.SingleOrDefault(expression);
                   return data;
               });
            //Action
            KeyNotFoundException result = Assert.Throws<KeyNotFoundException>(() => _accountService.ReadOneAccountByNumber(accountNumber));

            //assert
            Assert.NotNull(result);
        }
        #endregion

        #region Check Existed Account
        [Theory]
        [MemberData(nameof(CheckAccountExisted_ExistedNoPassword))]
        public void GivenExistedAccountWithoutPassword_WhenCheckExistedAccount_True(int? accountId, string userId, string password = "")
        {
            //arrange 
            Account foundEntity = new Account();

            _accountRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<Account, bool>>>())).
               Returns((Expression<Func<Account, bool>> exp) => {
                   var data = AccountFakeDb.Where(exp);
                   foundEntity = AccountFakeDb.First(exp);
                   return data;
               });
            //Action
            var result = _accountService.IsAccountExisted(accountId, userId, password);

            //assert
            Assert.Equal(true, result);
            Assert.True(foundEntity.Id == accountId
               && foundEntity.UserId == userId
               && foundEntity.Password != password);
        }

        [Theory]
        [MemberData(nameof(CheckAccountExisted_ExistedWithPassword))]
        public void GivenExistedAccountWithPassword_WhenCheckExistedAccount_True(int? accountId, string userId, string password = "")
        {
            //arrange 
            Account foundEntity= new Account();

            _accountRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<Account, bool>>>())).
               Returns((Expression<Func<Account, bool>> exp) => {
                   var data = AccountFakeDb.Where(exp);

                   foundEntity = AccountFakeDb.First(exp);

                   return data;
               });
            //Action
            var result = _accountService.IsAccountExisted(accountId, userId, password);

            //assert
            Assert.Equal(true, result);
            Assert.True(foundEntity.Id == accountId 
                && foundEntity.UserId == userId
                && foundEntity.Password == PasswordHelper.HashPassword(password));
        }

        [Theory]
        [MemberData(nameof(CheckAccountExisted_Unexisted))]
        public void GivenUnexistedAccount_WhenCheckExistedAccount_False(int? accountId, string userId, string password = "")
        {
            //arrange 
            _accountRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<Account, bool>>>())).
                Returns((Expression<Func<Account, bool>> expression) => {
                    var data = AccountFakeDb.Where(expression);
                    return data;
                });
            //Action

            var result = _accountService.IsAccountExisted(accountId, userId, password);

            //assert
            Assert.Equal(false, result);
        }

        [Theory]
        [InlineData(new object[] { 0, 1 })]
        [InlineData(new object[] { -1, 1 })]
        public void GivenInvalidAccountId_WhenCheckExistedAccount_ThrowArgumentException(int? accountId, string userId, string password = "")
        {
            //arrange 
            //Action
            ArgumentException result = Assert.Throws<ArgumentException>(() => _accountService.IsAccountExisted(accountId, userId, password));


            //assert
            Assert.Equal("accountId", result.ParamName);
        }


        [Theory]
        [InlineData(new object[] { 2, "" })]
        [InlineData(new object[] { 1, null })]
        [InlineData(new object[] { 3, "  " })]
        public void GivenInvalidUserId_WhenCheckExistedAccount_ThrowArgumentException(int? accountId, string userId, string password = "")
        {
            //arrange 
            //Action
            ArgumentException result = Assert.Throws<ArgumentException>(() => _accountService.IsAccountExisted(accountId, userId, password));


            //assert
            Assert.Equal("userId", result.ParamName);
        }
        #endregion

        #region Read transactions
        [Theory]
        [MemberData(nameof(ReadTransactions_Valid))]
        public void GivenValidData_WhenReadTransaction_Transactions(string userId, int accountId, int index, int itemPerPage)
        {
            //arrange 
            _transactionRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<TransactionHistory, bool>>>()))
                .Returns((Expression<Func<TransactionHistory, bool>> exp) => {
                    var data = TransactionFakeDb.Where(exp);
                    return data;
                });
            // Action
            int total = 0;
            var result = _accountService.ReadHistory(userId, accountId, index, itemPerPage, out total);

            //assert
            Assert.True(((index - 1) * itemPerPage > total && result.Count == 0)
                || ((index - 1) * itemPerPage <= total && result.Count != 0));
        }

        [Theory]
        [MemberData(nameof(ReadTransactions_Invalid))]
        public void GivenInvalidData_WhenReadTransactions_NoTransactions(string userId, int accountId, int index, int itemPerPage)
        {
            //arrange
            int total = 0;
            _transactionRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<TransactionHistory, bool>>>()))
              .Verifiable();
            List<string> paramErrors = new List<string>() { "userId", "accountId", "index", "itemPerPage" };
            //action
            ArgumentException result = Assert.Throws<ArgumentException>(() => _accountService.ReadHistory(userId, accountId, index, itemPerPage, out total));

            //assert
            Assert.True(total == 0);
            Assert.NotNull(result);
            Assert.True(paramErrors.Contains(result.ParamName));

            _transactionRepoMock.Verify(a => a.Read(It.IsAny<Expression<Func<TransactionHistory, bool>>>()), Times.Never);
        }
        #endregion
    }
}
