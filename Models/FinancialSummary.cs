using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NETFinalProject.Models
{
    public class FinancialSummary
    {
        //public virtual User User { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal CurrentBalance => TotalIncome - TotalExpense;

        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }

        public virtual List<Income> Incomes { get; set; } = new List<Income>();
        public virtual List<Expense> Expenses { get; set; } = new List<Expense>();


        public decimal GetCurrentBalance()
        {
            //CalculateTotals();
            return CurrentBalance;
        }

        public decimal GetBalanceForDateRange(DateTime startDate, DateTime endDate)
        {
            var incomesInRange = Incomes.Where(i => i.DateReceived >= startDate && i.DateReceived <= endDate).ToList();
            var expensesInRange = Expenses.Where(e => e.DateIncurred >= startDate && e.DateIncurred <= endDate).ToList();

            return incomesInRange.Sum(i => i.Amount) - expensesInRange.Sum(e => e.Amount);
        }

        //public void CalculateTotals()
        //{
        //    TotalIncome = Incomes.Sum(i => i.Amount);
        //    TotalExpense = Expenses.Sum(e => e.Amount);
        //}

        public void AddIncome(Income income)
        {
            Incomes.Add(income);
            //CalculateTotals();
            TotalIncome += income.Amount;
        }

        public void AddExpense(Expense expense)
        {
            Expenses.Add(expense);
            //CalculateTotals();
            TotalExpense += expense.Amount;
        }
    }
}
