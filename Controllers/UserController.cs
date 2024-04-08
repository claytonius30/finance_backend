using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETFinalProject.Data;
using NETFinalProject.Models;

namespace NETFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FinanceContext _context;

        public UserController(FinanceContext context)
        {
            _context = context;
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
        public async Task<ActionResult<User>> GetUser(int userId)
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
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes(int userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Incomes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var incomes = user.FinancialSummary.Incomes.OrderByDescending(i => i.DateReceived);
            return Ok(incomes);
        }

        // GET: api/User/{userId}/GetIncomesForDateRange
        [HttpGet("{userId}/GetIncomesForDateRange")]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomesForDateRange(int userId, DateTime startDate, DateTime endDate)
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
        public async Task<ActionResult<User>> GetIncome(int userId, int incomeId)
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
        public async Task<IActionResult> AddIncome(int userId, [FromBody] Income income)
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
        public async Task<IActionResult> UpdateIncome(int userId, int incomeId, Income updatedIncome)
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
        public async Task<IActionResult> DeleteIncome(int userId, int incomeId)
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
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(int userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                    .ThenInclude(fs => fs.Expenses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var expenses = user.FinancialSummary.Expenses.OrderByDescending(i => i.DateIncurred);
            return Ok(expenses);
        }

        // GET: api/User/{userId}/GetExpensesForDateRange
        [HttpGet("{userId}/GetExpensesForDateRange")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesForDateRange(int userId, DateTime startDate, DateTime endDate)
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
        public async Task<ActionResult<User>> GetExpense(int userId, int expenseId)
        {
            var user = await _context.Users
            .Include(u => u.FinancialSummary)
                .ThenInclude(fs => fs.Expenses)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }

            var income = user.FinancialSummary.Expenses.FirstOrDefault(i => i.ExpenseId == expenseId);
            if (income == null)
            {
                return NotFound();
            }

            return Ok(income);
        }

        // POST: api/User/{userId}/AddExpense
        [HttpPost("{userId}/AddExpense")]
        public async Task<IActionResult> AddExpense(int userId, [FromBody] Expense expense)
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

        // PUT: api/User/{userId}/UpdateExpense/{incomeId}
        [HttpPut("{userId}/UpdateExpense/{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int userId, int expenseId, Expense updatedExpense)
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
        public async Task<IActionResult> DeleteExpense(int userId, int expenseId)
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

        // GET: api/User/{userId}/GetCurrentBalance
        [HttpGet("{userId}/GetCurrentBalance")]
        public async Task<ActionResult<User>> GetCurrentBalance(int userId)
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
        public async Task<ActionResult<User>> GetBalanceForDateRange(int userId, DateTime startDate, DateTime endDate)
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
        public async Task<bool> CheckFinancialSummary(int userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user.FinancialSummary == null)
            {
                return false;
            }
            return true;
        }

        // GET: api/User/{userId}/GetAllTransactions
        [HttpGet("{userId}/GetAllTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions(int userId)
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

            // Add all incomes
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

            // Add all expenses
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

            // Order transactions by date
            transactions = transactions.OrderBy(t => t.Date).Reverse().ToList();

            return Ok(transactions);
        }

        // GET: api/User/{userId}/GetTransactionsForDateRange
        [HttpGet("{userId}/GetTransactionsForDateRange")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsForDateRange(int userId, DateTime startDate, DateTime endDate)
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
                if (income.DateReceived >= startDate && income.DateReceived <= endDate)
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
                if (expense.DateIncurred >= startDate && expense.DateIncurred <= endDate)
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
            transactions = transactions.OrderBy(t => t.Date).Reverse().ToList();

            return Ok(transactions);
        }

        // PUT: api/User/{userId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(int userId, User user)
        {
            if (userId != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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
            return Ok(user);
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
        public async Task<IActionResult> DeleteUser(int userId)
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

        private bool UserExists(int userId)
        {
            return _context.Users.Any(e => e.Id == userId);
        }
    }
}
