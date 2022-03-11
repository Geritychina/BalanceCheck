using BalanceCheck.Data;
using BalanceCheck.Models;
using BalanceCheck.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace BalanceCheck.Controllers
{
    public class IncomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public IncomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {      
            _context = context;
            _userManager = userManager;

           
        }
        // GET: Income
        public ActionResult Index()
        {
            var incomes = _context.Incomes.ToList();

            var userId = _userManager.GetUserId(HttpContext.User);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;

            var model =  _context.Incomes
                                     .Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                                     .ToList();



            





            return View(model);
            

        }

        [HttpGet]
        public ActionResult Send()
        {
            var incomes = _context.Incomes
                                         .Where(a => a.User.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                                         .ToList();

            decimal sumOfIncomes = 0;

            foreach (var modelIncome in incomes)
            {
                sumOfIncomes = sumOfIncomes + modelIncome.IncomeValue;
            }

            TempData["incomes"] = sumOfIncomes;

            return View(sumOfIncomes);
        }


            // GET: Income/Details/5'
            [HttpGet]
        public ActionResult Details(int id)
        {
            
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var incomeDb = _context.Incomes.Find(id);

            if (incomeDb == null)
            {
                return NotFound();
            }

            return View(incomeDb);
        }

        [HttpPost]
        public ActionResult Details(IncomeVM incomeVM)
        {

            if (ModelState.IsValid)
            {
                TempData["ResultOk"] = "Data Displayed Successfully !";
                return RedirectToAction("Index");
            }

            return View(incomeVM);
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
        public ActionResult Create(IncomeVM incomeVM)
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;
            

            if (ModelState.IsValid)
            {
                Income income = new Income
                {
                    IncomeValue = incomeVM.IncomeValue,
                    IsRepeated = incomeVM.IsRepeated,
                    IncomeDate = incomeVM.IncomeDate,
                    EndIncomeDate = incomeVM.EndIncomeDate,
                    User = user
                    
                };

                _context.Add(income);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }
            return View();
        }

        // GET: Income/Edit/5
        public ActionResult Edit(int id)
        {
         
            if (id == 0)
            {
                return NotFound();
            }
            var incomeDb = _context.Incomes.Find(id);

            if (incomeDb == null)
            {
                return NotFound();
            }

            return View(incomeDb);
        }

        // POST: Income/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncomeVM incomeVM)
        {
            

            if (ModelState.IsValid)
            {
                Income income = new Income
                {
                    IncomeId = incomeVM.IncomeId,
                    IncomeValue = incomeVM.IncomeValue,
                    IncomeDate = incomeVM.IncomeDate,
                    EndIncomeDate = incomeVM.EndIncomeDate,
                    IsRepeated = incomeVM.IsRepeated
                    
                };

                _context.Incomes.Update(income);
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
                Income income = new Income();
                income.IncomeId = id;
                
                _context.Remove(income);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
