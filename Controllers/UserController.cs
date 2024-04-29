using BackendFinance.Data;
using BackendFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly FinanceTrackerDbContext _context;

        public UserController(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/User/{email}/GetGuid
        [HttpGet("{email}/GetGuid")]
        public async Task<ActionResult<Guid>> GetGuid(string email)
        {
            Guid userId;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                userId = user.Id;
            }

            return Ok(userId);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _context.Database.EnsureCreated();
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/User/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);


            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/User/{userId}/GetIncomes
        [HttpGet("{userId}/GetIncomes")]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var incomes = user.FinancialSummary.Incomes.OrderBy(i => i.DateReceived);
            return Ok(incomes);
        }

        // GET: api/User/{userId}/GetIncomesForDateRange
        [HttpGet("{userId}/GetIncomesForDateRange")]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomesForDateRange(Guid userId, DateTime startDate, DateTime endDate)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var incomes = user.FinancialSummary.Incomes
                .Where(i => i.DateReceived >= startDate && i.DateReceived <= endDate)
                .OrderByDescending(i => i.DateReceived);
            return Ok(incomes);
        }

        // GET: api/User/{userId}/GetIncome/{incomeId}
        [HttpGet("{userId}/GetIncome/{incomeId}")]
        public async Task<ActionResult<User>> GetIncome(Guid userId, int incomeId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var income = user.FinancialSummary.Incomes.FirstOrDefault(i => i.IncomeId == incomeId);
            if (income == null)
            {
                return NotFound();
            }

            return Ok(income);
        }

        // POST: api/User/{userId}/AddIncome
        [HttpPost("{userId}/AddIncome")]
        public async Task<IActionResult> AddIncome(Guid userId, [FromBody] Income income)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            user.FinancialSummary ??= new FinancialSummary();

            user.FinancialSummary.AddIncome(income);
            await _context.SaveChangesAsync();
            return Ok(income);
        }

        // PUT: api/User/{userId}/UpdateIncome/{incomeId}
        [HttpPut("{userId}/UpdateIncome/{incomeId}")]
        public async Task<IActionResult> UpdateIncome(Guid userId, int incomeId, Income updatedIncome)
        {
            if (incomeId != updatedIncome.IncomeId || userId != updatedIncome.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var incomeToUpdate = user.FinancialSummary.Incomes.FirstOrDefault(i => i.IncomeId == incomeId);
            if (incomeToUpdate == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteIncome(incomeToUpdate);

            incomeToUpdate.Amount = updatedIncome.Amount;
            incomeToUpdate.Source = updatedIncome.Source;
            incomeToUpdate.DateReceived = updatedIncome.DateReceived;

            user.FinancialSummary.AddIncome(updatedIncome);

            await _context.SaveChangesAsync();

            return Ok(incomeToUpdate);
        }

        // DELETE: api/User/{userId}/DeleteIncome/{incomeId}
        [HttpDelete("{userId}/DeleteIncome/{incomeId}")]
        public async Task<IActionResult> DeleteIncome(Guid userId, int incomeId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var incomeToDelete = user.FinancialSummary.Incomes.FirstOrDefault(i => i.IncomeId == incomeId);
            if (incomeToDelete == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteIncome(incomeToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/User/{userId}/GetExpenses
        [HttpGet("{userId}/GetExpenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expenses = user.FinancialSummary.Expenses.OrderBy(i => i.DateIncurred);
            return Ok(expenses);
        }

        // GET: api/User/{userId}/GetExpensesForDateRange
        [HttpGet("{userId}/GetExpensesForDateRange")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesForDateRange(Guid userId, DateTime startDate, DateTime endDate)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expenses = user.FinancialSummary.Expenses
                .Where(e => e.DateIncurred >= startDate && e.DateIncurred <= endDate)
                .OrderByDescending(i => i.DateIncurred);
            return Ok(expenses);
        }

        // GET: api/User/{userId}/GetExpense/{expenseId}
        [HttpGet("{userId}/GetExpense/{expenseId}")]
        public async Task<ActionResult<User>> GetExpense(Guid userId, int expenseId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expense = user.FinancialSummary.Expenses.FirstOrDefault(i => i.ExpenseId == expenseId);
            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // POST: api/User/{userId}/AddExpense
        [HttpPost("{userId}/AddExpense")]
        public async Task<IActionResult> AddExpense(Guid userId, [FromBody] Expense expense)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            user.FinancialSummary ??= new FinancialSummary();
            user.FinancialSummary.AddExpense(expense);
            await _context.SaveChangesAsync();
            return Ok(expense);
        }

        // PUT: api/User/{userId}/UpdateExpense/{expenseId}
        [HttpPut("{userId}/UpdateExpense/{expenseId}")]
        public async Task<IActionResult> UpdateExpense(Guid userId, int expenseId, Expense updatedExpense)
        {
            if (expenseId != updatedExpense.ExpenseId || userId != updatedExpense.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expenseToUpdate = user.FinancialSummary.Expenses.FirstOrDefault(i => i.ExpenseId == expenseId);
            if (expenseToUpdate == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteExpense(expenseToUpdate);

            expenseToUpdate.Amount = updatedExpense.Amount;
            expenseToUpdate.Category = updatedExpense.Category;
            expenseToUpdate.DateIncurred = updatedExpense.DateIncurred;

            user.FinancialSummary.AddExpense(updatedExpense);

            await _context.SaveChangesAsync();

            return Ok(expenseToUpdate);
        }

        // DELETE: api/User/{userId}/DeleteExpense/{expenseId}
        [HttpDelete("{userId}/DeleteExpense/{expenseId}")]
        public async Task<IActionResult> DeleteExpense(Guid userId, int expenseId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expenseToDelete = user.FinancialSummary.Expenses.FirstOrDefault(e => e.ExpenseId == expenseId);
            if (expenseToDelete == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteExpense(expenseToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/User/{userId}/GetGoals
        [HttpGet("{userId}/GetGoals")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoals(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Goals)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var goals = user.FinancialSummary.Goals.OrderBy(i => i.Date);
            user.FinancialSummary.SetGoalsStatus();
            return Ok(goals);
        }

        // GET: api/User/{userId}/GetGoal/{goalId}
        [HttpGet("{userId}/GetGoal/{goalId}")]
        public async Task<ActionResult<User>> GetGoal(Guid userId, int goalId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Goals)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var goal = user.FinancialSummary.Goals.FirstOrDefault(i => i.GoalId == goalId);
            if (goal == null)
            {
                return NotFound();
            }
            goal.Status = user.FinancialSummary.SetGoalStatus(goal);
            return Ok(goal);
        }

        //// GET: api/User/{userId}/GetGoalsStatus
        //[HttpGet("{userId}/GetGoalsStatus/{goalId}")]
        //public async Task<ActionResult<User>> GetGoalsStatus(Guid userId, int goalId)
        //{
        //    var user = await _context.Users
        //        .Include(u => u.FinancialSummary)
        //            .ThenInclude(fs => fs.Goals)
        //        .FirstOrDefaultAsync(u => u.Id == userId);

        //    if (user == null || user.FinancialSummary == null)
        //    {
        //        return NotFound();
        //    }

        //    var goal = user.FinancialSummary.Goals.FirstOrDefault(i => i.GoalId == goalId);
        //    if (goal == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(goal);
        //}

        // POST: api/User/{userId}/AddGoal
        [HttpPost("{userId}/AddGoal")]
        public async Task<IActionResult> AddGoal(Guid userId, [FromBody] Goal goal)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            user.FinancialSummary ??= new FinancialSummary();
            user.FinancialSummary.AddGoal(goal);
            await _context.SaveChangesAsync();
            return Ok(goal);
        }

        // PUT: api/User/{userId}/UpdateGoal/{goalId}
        [HttpPut("{userId}/UpdateGoal/{goalId}")]
        public async Task<IActionResult> UpdateGoal(Guid userId, int goalId, Goal updatedGoal)
        {
            if (goalId != updatedGoal.GoalId || userId != updatedGoal.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Goals)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var goalToUpdate = user.FinancialSummary.Goals.FirstOrDefault(i => i.GoalId == goalId);
            if (goalToUpdate == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteGoal(goalToUpdate);

            goalToUpdate.Date = updatedGoal.Date;
            goalToUpdate.Amount = updatedGoal.Amount;
            goalToUpdate.Description = updatedGoal.Description;
            goalToUpdate.Status = user.FinancialSummary.SetGoalStatus(updatedGoal);

            user.FinancialSummary.AddGoal(updatedGoal);

            await _context.SaveChangesAsync();

            return Ok(goalToUpdate);
        }

        // DELETE: api/User/{userId}/DeleteGoal/{goalId}
        [HttpDelete("{userId}/DeleteGoal/{goalId}")]
        public async Task<IActionResult> DeleteGoal(Guid userId, int goalId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Goals)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var goalToDelete = user.FinancialSummary.Goals.FirstOrDefault(e => e.GoalId == goalId);
            if (goalToDelete == null)
            {
                return NotFound();
            }

            user.FinancialSummary.DeleteGoal(goalToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/User/{userId}/GetCurrentBalance
        [HttpGet("{userId}/GetCurrentBalance")]
        public async Task<ActionResult<User>> GetCurrentBalance(Guid userId)
        {
            decimal currentBalance = 0;
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            //if (user == null || user.FinancialSummary == null)
            if (user == null)
            {
                return NotFound();
            }
            if (user.FinancialSummary != null)
            {
                currentBalance = user.FinancialSummary.GetCurrentBalance();
            }

            return Ok(currentBalance);
        }

        // GET: api/User/{userId}/GetBalanceForDateRange
        [HttpGet("{userId}/GetBalanceForDateRange")]
        public async Task<ActionResult<User>> GetBalanceForDateRange(Guid userId, DateTime startDate, DateTime endDate)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }
            var dateRangeBalance = user.FinancialSummary.GetBalanceForDateRange(startDate, endDate);
            return Ok(dateRangeBalance);
        }

        // GET: api/User/{userId}/CheckFinancialSummary
        [HttpGet("{userId}/CheckFinancialSummary")]
        public async Task<bool> CheckFinancialSummary(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user?.FinancialSummary == null)
            {
                return false;
            }
            return true;
        }

        // GET: api/User/{userId}/GetAllTransactions
        [HttpGet("{userId}/GetAllTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var transactions = new List<Transaction>();

            // Adds all incomes
            foreach (var income in user.FinancialSummary.Incomes)
            {
                transactions.Add(new Transaction
                {
                    Type = "Income",
                    Id = income.IncomeId,
                    Amount = income.Amount,
                    Description = income.Source,
                    Date = income.DateReceived
                });
            }

            // Adds all expenses
            foreach (var expense in user.FinancialSummary.Expenses)
            {
                transactions.Add(new Transaction
                {
                    Type = "Expense",
                    Id = expense.ExpenseId,
                    Amount = -expense.Amount, // Expenses are considered negative amounts
                    Description = expense.Category,
                    Date = expense.DateIncurred
                });
            }

            // Orders transactions by date
            transactions = transactions.OrderBy(t => t.Date).ToList();

            return Ok(transactions);
        }

        // GET: api/User/{userId}/GetTransactionsForDateRange
        [HttpGet("{userId}/GetTransactionsForDateRange")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsForDateRange(Guid userId, DateTime startDate, DateTime endDate)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var transactions = new List<Transaction>();

            // Add all incomes within the date range
            foreach (var income in user.FinancialSummary.Incomes)
            {
                if (income.DateReceived >= startDate && income.DateReceived <= endDate.AddDays(1))
                {
                    transactions.Add(new Transaction
                    {
                        Type = "Income",
                        Id = income.IncomeId,
                        Amount = income.Amount,
                        Description = income.Source,
                        Date = income.DateReceived
                    });
                }
            }

            // Add all expenses within the date range
            foreach (var expense in user.FinancialSummary.Expenses)
            {
                if (expense.DateIncurred >= startDate && expense.DateIncurred <= endDate.AddDays(1))
                {
                    transactions.Add(new Transaction
                    {
                        Type = "Expense",
                        Id = expense.ExpenseId,
                        Amount = -expense.Amount, // Expenses are considered negative amounts
                        Description = expense.Category,
                        Date = expense.DateIncurred
                    });
                }
            }

            // Order transactions by date
            transactions = transactions.OrderBy(t => t.Date).ToList();

            return Ok(transactions);
        }

        // PUT: api/User/{userId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(Guid userId, User user)
        {
            if (userId != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;


            await _context.SaveChangesAsync();

            //_context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return Ok(existingUser);
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, user);
        }

        // DELETE: api/User/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid userId)
        {
            return _context.Users.Any(e => e.Id == userId);
        }
    }
}
