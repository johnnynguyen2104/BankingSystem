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
        [Theory]
        [MemberData(nameof(CreationAccount_Success))]
        public void GivenValidAccountInfo_WhenCreate_Success(AccountDto entity)
        {
            //arrange & Action
            var result = _accountService.Create(entity);
            
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(CreationAccount_Null))]
        public void GivenNullOrEmmty_Password_AccountName_AccountNumber_WhenCreate_ReturnNull(AccountDto entity)
        {
            //arrange & Action
            var result = _accountService.Create(entity);

            //assert
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(UpdateBalance_Success))]
        public void GivenValidData_WhenUpdateBalance_Success(int accountId, double value, string userId)
        {
            //arrange & Action
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
    }
}
