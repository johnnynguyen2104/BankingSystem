using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Controllers
{
    public class BaseController : Controller
    {
        public void EndTransaction(int accountId)
        {
            TempData.Remove($"account_{accountId}");
        }

        public void EndAllTransaction()
        {
            TempData.Clear();
        }
    }
}
