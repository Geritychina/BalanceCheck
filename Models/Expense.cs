using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BalanceCheck.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [DataType(DataType.Currency)]
        public decimal ExpenseValue { get; set; }

        public Boolean IsRepeated { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime? EndExpenseDate { get; set; }

        public IdentityUser User { get; set; }
    }
}
