namespace BalanceCheck.ViewModels
{
    public class IncomeVM
    {
        public int IncomeId { get; set; }
        public decimal IncomeValue { get; set; }
        public Boolean IsRepeated { get; set; }
        public DateTime IncomeDate { get; set; }

        public DateTime? EndIncomeDate { get; set; }


    }
}
