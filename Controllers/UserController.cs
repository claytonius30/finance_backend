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

        // GET: api/User/5
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
            return Ok(user.FinancialSummary);
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
            return Ok(user.FinancialSummary);
        }

        // GET: api/User/{userId}/GetCurrentBalance
        [HttpGet("{userId}/GetCurrentBalance")]
        public async Task<ActionResult<User>> GetCurrentBalance(int userId)
        {
            var user = await _context.Users
                .Include(u => u.FinancialSummary)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.FinancialSummary == null)
            {
                return NotFound();
            }
            var currentBalance = user.FinancialSummary.GetCurrentBalance();
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

        // PUT: api/User/5
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

        // DELETE: api/User/5
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
