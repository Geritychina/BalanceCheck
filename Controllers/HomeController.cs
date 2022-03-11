using BalanceCheck.Data;
using BalanceCheck.Models;
using BalanceCheck.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace BalanceCheck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;

            decimal balance = 0;

            if (user != null)
            {
                var incomesDb = _context.Incomes
                                     .Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                                     .ToList();

                var expensesDb = _context.Expenses
                                    .Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                                    .ToList();


                foreach (var incomeModel in incomesDb)
                {

                    if (incomeModel.IsRepeated == true)
                    {

                        var incomeCreatedDate = incomeModel.IncomeDate;
                        var incomeEndsDate = (DateTime)incomeModel.EndIncomeDate;

                        /*TimeSpan daysAppart = incomeEndsDate-incomeCreatedDate ;
                        int y = (int)((int)daysAppart.TotalDays / 30.43666666666667);*/

                        //balance = balance + y * incomeModel.IncomeValue;

                        /*int monthsApart = Math.Abs( 12 * (incomeCreatedDate.Year - incomeEndsDate.Year) + incomeCreatedDate.Month - incomeEndsDate.Month);

                        var x = monthsApart * 30.43;*/

                        while (incomeCreatedDate <= incomeModel.EndIncomeDate)
                        {
                            var placeholderIncomeDate = incomeCreatedDate.AddMonths(1);
                            incomeCreatedDate = placeholderIncomeDate;
                            
                            balance = balance + incomeModel.IncomeValue;
                        }
                        
                    }

                    if(incomeModel.IsRepeated == false)
                    {
                        balance = balance + incomeModel.IncomeValue;
                    }

                }

                foreach (var expenseModel in expensesDb)
                {
                    if (expenseModel.IsRepeated == true)
                    {
                        var expenseCreatedDate = expenseModel.ExpenseDate;
                        var expenseEndsDate = (DateTime)expenseModel.EndExpenseDate;

                        //int monthsApart = Math.Abs(12 * (expenseCreatedDate.Year - expenseEndsDate.Year) + expenseCreatedDate.Month - expenseEndsDate.Month);

                        //balance = balance - (monthsApart) * expenseModel.ExpenseValue;

                        while (expenseCreatedDate <= expenseModel.EndExpenseDate)
                        {
                            var placeholderExpenseDate = expenseCreatedDate.AddMonths(1);
                            expenseCreatedDate = placeholderExpenseDate;

                            balance = balance - expenseModel.ExpenseValue;
                        }
                    }

                    if (expenseModel.IsRepeated == false)
                    {
                        balance = balance - expenseModel.ExpenseValue;
                    }

                   
                }
            }

            TempData["balance"] = balance;
            TempData["balance"] = JsonConvert.SerializeObject(balance);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}