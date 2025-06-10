using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyController : Controller
    {
        private readonly ManagementDbContext _context;
        public MoneyController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoney()
        {

            var money = await _context.Money.ToListAsync();

            if (money == null || !money.Any())
            {
                return NotFound();
            }

            return Ok(money);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMoney(long Id)
        {
            var money = await _context.Money.FindAsync(Id);

            if (money == null)
            {
                return NotFound();
            }

            return Ok(money);
        }

        [HttpPost]
        public async Task<IActionResult> PostMoney(Money money)
        {
            try
            {
                _context.Money.Add(money);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsMoneyExists(money.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostMoney), money);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutMoney(long Id, Money money)
        {
            if (Id != money.ID) return BadRequest("Ids does not match!");
            if (!await IsMoneyExists(Id)) return NotFound();
            try
            {
                _context.Entry(money).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteMoney(long Id)
        {
            if (!await IsMoneyExists(Id))
                return NotFound();
            try
            {
                var money = await _context.Money.FindAsync(Id);
                _context.Money.Remove(money);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsMoneyExists(long Id)
        {
            bool exists = await _context.Money.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
