using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BalanceCheck.Models
{
    public class AppUser:IdentityUser
    {
    
        public List<Income> Incomes { get; set; }

        public List<Expense> Expenses { get; set; }

    }
}
