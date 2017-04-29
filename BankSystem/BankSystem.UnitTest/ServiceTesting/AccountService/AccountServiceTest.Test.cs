using BankSystem.DAL.DomainModels;
using BankSystem.Service.Dtos;
using Moq;
using System;
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
            // Action
            var result = _accountService.UpdateBalance(value, accountId, userId);

            //assert
            Assert.Equal(true, result);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_Fail))]
        public void GivenInValidData_WhenUpdateBalance_False(int accountId, double value, string userId)
        {
            //arrange & Action
            var result = _accountService.UpdateBalance(value, accountId, userId);

            //assert
            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_Throw))]
        public void GivenInValidAccount_User_NotEnoughMoney_WhenUpdateBalance_ThrowException(int accountId, double value, string userId)
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()));
            Exception ex = Assert.Throws<Exception>(() => _accountService.UpdateBalance(value, accountId, userId));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
        }
        #endregion

        #region Transfer
        [Theory]
        [MemberData(nameof(Transfer_Throw))]
        public void GivenInvalidAccount_WhenTransfer_ThrowException(int accountId, double value, int desAccountId)
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>()));
            Exception ex = Assert.Throws<Exception>(() => _accountService.TransferMoney(accountId, value, desAccountId));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
        }

        [Theory]
        [MemberData(nameof(Transfer_Throw))]
        public void GivenNotEnoughMoney_WhenTransfer_ThrowException(int accountId, double value, int desAccountId)
        {
            //arrange & Action
            _accountRepoMock.Setup(x => x.ReadOne(It.IsAny<Expression<Func<Account, bool>>>())).Returns(new Account() { Balance = 0 });
            Exception ex = Assert.Throws<Exception>(() => _accountService.TransferMoney(accountId, value, desAccountId));

            //assert
            Assert.NotNull(ex);
            Assert.NotEmpty(ex.Message);
        }

        [Theory]
        [MemberData(nameof(Transfer_False))]
        public void GivenInvalidValue_WhenTransfer_ReturnFalse(int accountId, double value, int desAccountId)
        {
            //arrange & Action
            var result = _accountService.TransferMoney(accountId, value, desAccountId);

            //assert
            Assert.Equal(false, result);
        }

        [Theory]
        [MemberData(nameof(Transfer_True))]
        public void GivenValidData_WhenTransfer_ReturnTrue(int accountId, double value, int desAccountId)
        {
            //arrange & Action
            var result = _accountService.TransferMoney(accountId, value, desAccountId);

            //assert
            Assert.Equal(true, result);
        }
        #endregion

        #region Read accounts by number
        [Theory]
        [MemberData(nameof(ReadAccountByNumber_Valid))]
        public void GivenValidData_WhenReadAccountByNumber_Account(string accountNumber)
        {
            //arrange & Action
            var result = _accountService.ReadOneAccountByNumber(accountNumber);

            //assert
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(ReadAccountByNumber_Invalid))]
        public void GivenInvalidData_WhenReadAccountByNumber_ReturnNull(string accountNumber)
        {
            //arrange & Action
            var result = _accountService.ReadOneAccountByNumber(accountNumber);

            //assert
            Assert.Null(result);
        }
        #endregion

        #region Check Existed Account
        [Theory]
        [MemberData(nameof(CheckAccountExisted_Existed))]
        public void GivenExistedAccount_WhenCheckExistedAccount_True(int? accountId, string userId, string password = "")
        {
            //arrange & Action
            var result = _accountService.IsAccountExisted(accountId, userId, password);

            //assert
            Assert.Equal(true, result);
        }

        [Theory]
        [MemberData(nameof(CheckAccountExisted_Unexisted))]
        public void GivenUnexistedAccount_WhenCheckExistedAccount_False(int? accountId, string userId, string password = "")
        {
            //arrange & Action
            _accountRepoMock.Setup(a => a.Read(It.IsAny<Expression<Func<Account, bool>>>()));
            var result = _accountService.IsAccountExisted(accountId, userId, password);

            //assert
            Assert.Equal(false, result);
        }
        #endregion

        #region read accounts by userId
        [Theory]
        [MemberData(nameof(ReadAccount_Valid))]
        public void GivenValidData_WhenReadAccounts_Accounts(string userId)
        {
            //arrange & Action
            var result = _accountService.ReadAccount(userId);

            //assert
            Assert.True(result.Count > 0);
        }

        [Theory]
        [MemberData(nameof(ReadAccount_Invalid))]
        public void GivenValidData_WhenReadAccounts_NoAccounts(string userId)
        {
            //arrange & Action
            var result = _accountService.ReadAccount(userId);

            //assert
            Assert.True(result.Count == 0);
        }
        #endregion

        #region Read transactions
        [Theory]
        [MemberData(nameof(ReadTransactions_Invalid))]
        public void GivenValidData_WhenReadTransaction_Transactions(string userId, int accountId, int index, int itemPerPage)
        {
            //arrange & Action
            int total = 0;
            var result = _accountService.ReadHistory(userId, accountId, index, itemPerPage, out total);

            //assert
            Assert.True(result.Count == 0);
        }

        [Theory]
        [MemberData(nameof(ReadTransactions_Invalid))]
        public void GivenInvalidData_WhenReadTransactions_NoTransactions(string userId, int accountId, int index, int itemPerPage)
        {
            //arrange & Action
            int total = 0;
            var result = _accountService.ReadHistory(userId, accountId, index, itemPerPage, out total);

            //assert
            Assert.True(result.Count == 0);
        }
        #endregion
    }
}
