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

namespace BankSystem.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            var userId = User.Claims.SingleOrDefault(a => a.Type.Contains("nameidentifier"))?.Value;
            var result = _accountService.ReadAccount(userId);
            return View(result);
        }

        public IActionResult AccountCreationForm(AccountCreationVM vm)
        {
            TempData["ValidateAccountNumber"] = vm.AccountNumber;
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
                    dto.UserId = User.Claims.SingleOrDefault(a => a.Type.Contains("nameidentifier"))?.Value;
                    dto.Password = PasswordHelper.HashPassword(dto.Password);

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
        public IActionResult TransactionHistory(int? accountId)
        {

            return View();
        }

        [Route("/Withdraw/{accountId}")]
        public IActionResult WithdrawForm(int? accountId)
        {
            return View();
        }

        [Route("/Deposit/{accountId}")]
        public IActionResult DepositForm(int? accountId)
        {
            return View();
        }

        [Route("/FundTransfer/{accountId}")]
        public IActionResult FundTransferForm(int? accountId)
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
