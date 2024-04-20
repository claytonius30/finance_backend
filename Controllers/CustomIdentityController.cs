using BackendFinance.Data;
using BackendFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendFinance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomIdentityController : Controller
    {
        private readonly FinanceTrackerDbContext _context;

        public CustomIdentityController(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomIdentity/{email}/GetGuid
        [HttpGet("{email}/GetLockoutEnd")]
        public async Task<ActionResult<LockoutInfo>> GetLockoutEnd(string email)
        {
            //DateTime lockoutRemaining;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd == null)
            {
                return Ok(new LockoutInfo { LockoutEnd = "isNull", LockoutRemaining = "" });
            }
            else
            {
                DateTimeOffset lockoutEnd = (DateTimeOffset)user.LockoutEnd!;
                string formattedLockoutEnd = lockoutEnd.DateTime.ToString("dddd, M/d h:mm tt");
                
                var remaining = lockoutEnd.Subtract(DateTimeOffset.Now);
                var hours = 5 + remaining.Hours;
                var mins = 60 + remaining.Minutes;
                var tempMin = 0;
                if (hours < 0 || mins < 0)
                {
                    await ResetLockout(user);
                    return Ok(new LockoutInfo { LockoutEnd = "isNull", LockoutRemaining = "" });
                }
                if (mins >= 60)
                {
                    hours++;
                    tempMin = 60;
                }
                mins -= tempMin;
                string formattedLockoutRemaining = $"{hours} hrs {mins} mins remaining";

                var lockoutInfo = new LockoutInfo
                {
                    LockoutEnd = formattedLockoutEnd,
                    LockoutRemaining = formattedLockoutRemaining
                };

                return Ok(lockoutInfo);
            }
        }

        // PUT: api/CustomIdentity/ResetLockout
        [HttpPut("/ResetLockout")]
        public async Task<IActionResult> ResetLockout(User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (user == null)
            {
                return NotFound();
            }

            existingUser!.LockoutEnd = null;


            await _context.SaveChangesAsync();

            //_context.Entry(user).State = EntityState.Modified;

            

            //return NoContent();
            return Ok();
        }
    }
}
