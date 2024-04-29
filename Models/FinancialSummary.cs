using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BackendFinance.Models
{
    public class FinancialSummary
    {
        public decimal TotalIncome { get; set; } = decimal.Zero;
        public decimal TotalExpense { get; set; } = decimal.Zero;
        public decimal CurrentBalance => TotalIncome - TotalExpense;

        public virtual List<Income> Incomes { get; set; } = new List<Income>();
        public virtual List<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual List<Goal> Goals { get; set; } = new List<Goal>();


        public decimal GetCurrentBalance()
        {
            return CurrentBalance;
        }

        public decimal GetBalanceForDateRange(DateTime startDate, DateTime endDate)
        {
            var incomesInRange = Incomes.Where(i => i.DateReceived >= startDate && i.DateReceived <= endDate).ToList();
            var expensesInRange = Expenses.Where(e => e.DateIncurred >= startDate && e.DateIncurred <= endDate).ToList();

            return incomesInRange.Sum(i => i.Amount) - expensesInRange.Sum(e => e.Amount);
        }

        public void AddIncome(Income income)
        {
            Incomes.Add(income);
            TotalIncome += income.Amount;
        }

        public void DeleteIncome(Income income)
        {
            Incomes.Remove(income);
            TotalIncome -= income.Amount;
        }
        
        public void AddExpense(Expense expense)
        {
            Expenses.Add(expense);
            TotalExpense += expense.Amount;
        }

        public void DeleteExpense(Expense expense)
        {
            Expenses.Remove(expense);
            TotalExpense -= expense.Amount;
        }

        public void AddGoal(Goal goal)
        {
            goal.Status = "In progress";
            Goals.Add(goal);
        }

        public void DeleteGoal(Goal goal)
        {
            Goals.Remove(goal);
        }

        public void SetGoalsStatus()
        {
            foreach (var goal in Goals)
            {
                goal.Status = SetGoalStatus(goal);
            }
        }

        public string SetGoalStatus(Goal goal)
        {
            if (goal.Date <= DateTime.Now)
            {
                decimal goalDifference = CurrentBalance - goal.Amount;

                if (goalDifference >= 0)
                {
                    return $"Goal met by {String.Format("${0:N2}", goalDifference)}";
                }
                else
                {
                    return $"Missed goal by {String.Format("${0:N2}", -goalDifference)}";
                }
            }
            else
            {
                return "In progress";
            }
        }
    }
}
