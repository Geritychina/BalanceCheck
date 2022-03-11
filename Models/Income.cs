using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BalanceCheck.Models
{
    public class Income
    {
        [Key]
        public int IncomeId { get; set; }

        [DataType(DataType.Currency)]
        public decimal IncomeValue { get; set; }

        public Boolean IsRepeated { get; set; }

        public DateTime IncomeDate { get; set; }

        public DateTime? EndIncomeDate { get; set; }

        public IdentityUser User { get; set; }
    }
}
