using BalanceCheck.Data;
using BalanceCheck.Models;
using BalanceCheck.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BalanceCheck.Controllers
{
    public class ExpenseController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpenseController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ExpenseController
        public ActionResult Index()
        {
            var model = _context.Expenses
                                    .Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                                    .ToList();

            return View(model);
        }

        // GET: ExpenseController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var expenseDb = _context.Expenses.Find(id);

            if (expenseDb == null)
            {
                return NotFound();
            }

            return View(expenseDb);
        }

        [HttpPost]
        public ActionResult Details(ExpenseVM expenseVM)
        {

            if (ModelState.IsValid)
            {
                TempData["ResultOk"] = "Data Displayed Successfully !";
                return RedirectToAction("Index");
            }

            return View(expenseVM);
        }


        // GET: Income/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Income/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpenseVM expenseVM)
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;

            if (ModelState.IsValid)
            {
                Expense expense = new Expense
                {
                    ExpenseValue = expenseVM.ExpenseValue,
                    IsRepeated = expenseVM.IsRepeated,
                    ExpenseDate = expenseVM.ExpenseDate,
                    EndExpenseDate = expenseVM.EndExpenseDate,
                    User = user
                };

                _context.Add(expense);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }
            return View();
        }

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var expenseDb = _context.Expenses.Find(id);

            if (expenseDb == null)
            {
                return NotFound();
            }

            return View(expenseDb);
        }

        // POST: Income/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExpenseVM expenseVM)
        {
            if (ModelState.IsValid)
            {
                Expense expense = new Expense
                {
                    ExpenseId = expenseVM.ExpenseId,
                    ExpenseValue = expenseVM.ExpenseValue,
                    ExpenseDate = expenseVM.ExpenseDate,
                    EndExpenseDate = expenseVM.EndExpenseDate,
                    IsRepeated = expenseVM.IsRepeated
                };

                _context.Expenses.Update(expense);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View();

        }

        // GET: Income/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Income/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                Expense expense = new Expense();
                expense.ExpenseId = id;

                _context.Remove(expense);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();

        }

    }
}

