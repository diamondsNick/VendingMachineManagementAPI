using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendingMachineMoneyController : Controller
    {
        private readonly ManagementDbContext _context;
        public VendingMachineMoneyController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendingMachineMoney()
        {
            var vendingMoney = await _context.VendingMachineMoney
                .Include(vm => vm.VendingMachine)
                .Include(vm => vm.Money)
                .ToListAsync();

            if (vendingMoney == null || !vendingMoney.Any())
            {
                return NotFound();
            }

            return Ok(vendingMoney);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVendingMachineMoney(long Id)
        {
            var vendingMoney = await _context.VendingMachineMoney
                .Include(vm => vm.VendingMachine)
                .Include(vm => vm.Money)
                .FirstOrDefaultAsync(vm => vm.ID == Id);

            if (vendingMoney == null)
            {
                return NotFound();
            }

            return Ok(vendingMoney);
        }

        [HttpPost]
        public async Task<IActionResult> PostVendingMachineMoney(VendingMachineMoney vendingMoney)
        {
            try
            {
                _context.VendingMachineMoney.Add(vendingMoney);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsVendingMachineMoneyExists(vendingMoney.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostVendingMachineMoney), vendingMoney);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutVendingMachineMoney(long Id, VendingMachineMoney vendingMoney)
        {
            if (Id != vendingMoney.ID) return BadRequest("Ids do not match!");
            if (!await IsVendingMachineMoneyExists(Id)) return NotFound();
            try
            {
                _context.Entry(vendingMoney).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(vendingMoney);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteVendingMachineMoney(long Id)
        {
            if (!await IsVendingMachineMoneyExists(Id))
                return NotFound();
            try
            {
                var vendingMoney = await _context.VendingMachineMoney.FindAsync(Id);
                _context.VendingMachineMoney.Remove(vendingMoney);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsVendingMachineMoneyExists(long Id)
        {
            bool exists = await _context.VendingMachineMoney.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
