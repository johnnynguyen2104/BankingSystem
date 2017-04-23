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

namespace BankSystem.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IConfigurationRoot _configuration;

        public string UserId { get { return User.Claims.SingleOrDefault(a => a.Type.Contains("nameidentifier"))?.Value; } }

        private readonly AppSettings _appSettings;

        public AccountController(IAccountService accountService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _accountService = accountService;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            var result = _accountService.ReadAccount(UserId);
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
                    dto.UserId = UserId;
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
        public IActionResult TransactionHistory(int? accountId, int page)
        {
            int totalItem = 0;
            var pagingVM = new PagingVM<TransactionHistoryDto>()
            {
                Items = _accountService.ReadHistory(UserId, accountId ?? 0, page, _appSettings.ItemPerPage, out totalItem),
                Pager = new Pager(totalItem, page, _appSettings.ItemPerPage)
            };
            return View(pagingVM);
        }

        [Route("/Withdraw/{accountId}")]
        public IActionResult WithdrawForm(int? accountId, int page)
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
