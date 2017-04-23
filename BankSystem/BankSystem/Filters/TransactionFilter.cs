using BankSystem.Controllers;
using BankSystem.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Filters
{
    public class TransactionFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int? accountId = (int?)ClassHelper.GetProperty(context.ActionArguments, "accountid");
            var accountController = (AccountController)context.Controller;

            if (accountController.TempData[$"account_{accountId}"] == null)
            {
                context.Result = accountController.RedirectToAction("Index", new { errors = new List<string>() { "Invalid Transaction." } });
            }
            else
            {
                accountController.TempData.Keep($"account_{accountId}");
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
