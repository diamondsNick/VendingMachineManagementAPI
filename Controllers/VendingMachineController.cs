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
    public class VendingMachineController : Controller
    {
        private readonly ManagementDbContext _context;
        public VendingMachineController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendingMachines()
        {
            var machines = await _context.VendingMachines
                .Include(vm => vm.Status)
                .Include(vm => vm.OperatingMode)
                .Include(vm => vm.Company)
                .Include(vm => vm.Modem)
                .Include(vm => vm.MachinePaymentMethods)
                .Include(vm => vm.VendingMachineMoney)
                .Include(vm => vm.VendingAvaliabilities)
                .Include(vm => vm.VendingMachineMatrix)
                .ToListAsync();

            if (machines == null || !machines.Any())
            {
                return NotFound();
            }

            return Ok(machines);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVendingMachine(long Id)
        {
            var machine = await _context.VendingMachines
                .Include(vm => vm.Status)
                .Include(vm => vm.OperatingMode)
                .Include(vm => vm.Company)
                .Include(vm => vm.Modem)
                .Include(vm => vm.MachinePaymentMethods)
                .Include(vm => vm.VendingMachineMoney)
                .Include(vm => vm.VendingAvaliabilities)
                .Include(vm => vm.VendingMachineMatrix)
                .FirstOrDefaultAsync(vm => vm.ID == Id);

            if (machine == null)
            {
                return NotFound();
            }

            return Ok(machine);
        }

        [HttpPost]
        public async Task<IActionResult> PostVendingMachine(VendingMachine machine)
        {
            try
            {
                _context.VendingMachines.Add(machine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsVendingMachineExists(machine.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostVendingMachine), machine);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutVendingMachine(long Id, VendingMachine machine)
        {
            if (Id != machine.ID) return BadRequest("Ids do not match!");
            if (!await IsVendingMachineExists(Id)) return NotFound();
            try
            {
                _context.Entry(machine).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(machine);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteVendingMachine(long Id)
        {
            if (!await IsVendingMachineExists(Id))
                return NotFound();
            try
            {
                var machine = await _context.VendingMachines.FindAsync(Id);
                _context.VendingMachines.Remove(machine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsVendingMachineExists(long Id)
        {
            bool exists = await _context.VendingMachines.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
