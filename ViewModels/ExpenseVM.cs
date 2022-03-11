namespace BalanceCheck.ViewModels
{
    public class ExpenseVM
    {
        public int ExpenseId { get; set; }
        public decimal ExpenseValue { get; set; }
        public Boolean IsRepeated { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime? EndExpenseDate { get; set; }
    }
}
