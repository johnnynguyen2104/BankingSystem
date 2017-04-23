using BankSystem.Controllers;
using BankSystem.Helpers;
using BankSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BankSystem.Filters
{
    public class AccountFilter : ActionFilterAttribute
    {
        private readonly IAccountService _accountService;

        public AccountFilter(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            int? accountId = (int?)ClassHelper.GetProperty(context.ActionArguments, "accountid");
            string password = ClassHelper.GetProperty(context.ActionArguments, "password") as string;
            var userId = context.HttpContext.User.Claims.SingleOrDefault(a => a.Type.Contains("nameidentifier"))?.Value;
            

            if (accountId == null || accountId == 0 || !_accountService.IsAccountExisted(accountId, userId, password))
            {
                var controller = (AccountController)context.Controller;
                context.Result = controller.RedirectToAction("Index", new { errors = new List<string>() { "Invalid Account or Password." } });
            }
          
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
