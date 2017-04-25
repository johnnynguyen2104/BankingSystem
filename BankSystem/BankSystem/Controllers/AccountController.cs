using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankSystem.Models.AccountViewModels;
using BankSystem.Service.Interfaces;
using AutoMapper;
using BankSystem.Service.Dtos;
using BankSystem.Service.Helpers;
using Microsoft.Extensions.Configuration;
using BankSystem.Models;
using BankSystem.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using BankSystem.Security.IdentityBusiness;
using BankSystem.Security;
using System.Security.Claims;
using BankSystem.Filters;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IUserAuthBusiness _userAuthBusiness;

        public string UserId { get { return User.Claims.SingleOrDefault(a => a.Type.Contains("nameidentifier"))?.Value; } }

        private readonly AppSettings _appSettings;

        public AccountController(IAccountService accountService, IMapper mapper, IOptions<AppSettings> appSettings,
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            _userAuthBusiness = new UserAuthBusiness(signInManager, userManager, mapper);
            _mapper = mapper;
            _accountService = accountService;
            _appSettings = appSettings.Value;
        }

        [ServiceFilter(typeof(AccountFilter))]
        public IActionResult AccountLogin(AccountLoginVM login)
        {
            return View(login);
        }

        [ServiceFilter(typeof(AccountFilter))]
        public IActionResult Login(AccountLoginVM login)
        {
            //simulate a transaction
            TempData[$"account_{login.AccountId}"] = true;

            return RedirectToAction("TransactionHistory", new { accountId = login.AccountId, page = 0 });
        }

        public IActionResult Index(IList<string> errors = null, bool? transactionCompleted = null)
        {
            AddErrors(errors);
            EndAllTransaction();
            var vm = new IndexAccountVM() { Accounts = _accountService.ReadAccount(UserId), TransactionCompleted = transactionCompleted };
            return View(vm);
        }

        public IActionResult AccountCreationForm(AccountCreationVM vm)
        {
            //validate account number that we generate for user
            TempData["ValidateAccountNumber"] = vm.AccountNumber;
            vm.UserId = UserId;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AccountCreation(AccountCreationVM vm)
        {
            if (ModelState.IsValid)
            {
                //validate account number that we generate for user
                var validateAccountNumber = (TempData["ValidateAccountNumber"] ?? "").ToString();
                var dto = _mapper.Map<AccountDto>(vm);

                if (vm.AccountNumber == validateAccountNumber)
                {
                    var result = _accountService.Create(dto);

                    if (result != null)
                    {
                        return Redirect("Index");
                    }
                }
            }
            
            return RedirectToAction("AccountCreationForm", vm);
        }

        [Route("/AccountDetails/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult TransactionHistory(int? accountId, int page = 1)
        {

            int totalItem = 0;
            var pagingVM = new PagingVM<int, TransactionHistoryDto>()
            {
                Id = accountId.Value,
                Items = _accountService.ReadHistory(UserId, accountId.Value, page, _appSettings.ItemPerPage, out totalItem),
                Pager = new Pager(totalItem, page, _appSettings.ItemPerPage)
            };

            return View(pagingVM);
        }

        [Route("/Withdraw/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult WithdrawForm(WithdrawDepositVM vm, IList<string> errors)
        {
            AddErrors(errors);
            return View(vm);
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Withdraw(WithdrawDepositVM vm)
        {
            try
            {
                if (_accountService.UpdateBalance(vm.Money * -1, vm.AccountId))
                {
                    //end transaction
                    EndTransaction(vm.AccountId);
                    return RedirectToAction("Index", new { transactionCompleted = true });
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                return RedirectToAction("WithdrawForm", new { vm = vm, errors = new List<string>() { ex.Message } });
            }
            catch (Exception ex)
            {
                EndTransaction(vm.AccountId);
                return RedirectToAction("Index", new { errors = new List<string>() { ex.Message } });
            }

            return RedirectToAction("Index");
        }

        [Route("/Deposit/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult DepositForm(WithdrawDepositVM vm)
        {

            return View(vm);
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Deposit(WithdrawDepositVM vm)
        {
            try
            {
                if (_accountService.UpdateBalance(vm.Money, vm.AccountId))
                {
                    //end transaction
                    EndTransaction(vm.AccountId);
                    return RedirectToAction("Index", new { transactionCompleted = true });
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                return RedirectToAction("DepositForm", new { vm = vm, errors = new List<string>() { ex.Message } });
            }
            catch (Exception ex)
            {
                EndTransaction(vm.AccountId);
                return RedirectToAction("Index", new { errors = new List<string>() { ex.Message } });
            }

            return RedirectToAction("Index");
        }

        [Route("/FundTransfer/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult FundTransferForm(FundTransferVM vm)
        {

            return View(vm);
        }

        [HttpPost]
        [Route("/Confirm/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult FundTransferConfirm(FundTransferVM vm, IList<string> errors)
        {
            AddErrors(errors);
            var accountDto = _accountService.ReadOneAccountByNumber(vm.AccountDesNumber);

            if (accountDto == null)
            {
                AddErrors(new List<string>() { "Invalid account number." });
                vm.AccountDesNumber = "";
                return View("FundTransferForm", vm);
            }

            vm.AccountDesName = accountDto.AccountName;
            vm.AccountDestinationId = accountDto.Id;
            TempData["ValidateAccountDesId"] = accountDto.Id;

            return View(vm);
        }

        [HttpPost]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Transfer(FundTransferVM vm)
        {
            try
            {
                //for security, in case hacker change Destination accountId.
                if (!TempData.ContainsKey("ValidateAccountDesId") 
                    || (int)TempData["ValidateAccountDesId"] != vm.AccountDestinationId)
                {
                    EndTransaction(vm.AccountId);
                    return RedirectToAction("Index", new { errors = new List<string>() { "Somthing went wrong. Please try again." } });
                }

                if (_accountService.TransferMoney(vm.AccountId, vm.Value, vm.AccountDestinationId))
                {
                    //end transaction
                    EndTransaction(vm.AccountId);
                    return RedirectToAction("Index", new { transactionCompleted = true });
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                return RedirectToAction("FundTransferConfirm", new { vm = vm, errors = new List<string>() { ex.Message } });
            }
            catch (Exception ex)
            {
                EndTransaction(vm.AccountId);
                return RedirectToAction("Index", new { errors = new List<string>() { ex.Message } });
            }

            return RedirectToAction("Index");
        }

        [Route("/Balance/{accountId}")]
        [ServiceFilter(typeof(AccountFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult BalanceCheking(int? accountId)
        {
            var vm = new BalanceCheckingVM() { AccountId = accountId.Value, Balance = _accountService.ReadOneById(accountId.Value).Balance };
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }

        private void AddErrors(IList<string> errors)
        {
            foreach (var item in errors)
            {
                ModelState.AddModelError(string.Empty, item);
            }
        }
    }
}
