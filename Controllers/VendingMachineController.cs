using System;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs;
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
                .ThenInclude(mx => mx.Manufacturer)
                .ToListAsync();

            if (machines == null || !machines.Any())
            {
                return NotFound();
            }

            return Ok(machines);
        }

        [HttpGet("{CompanyId:long}/{amount:int}/{page:int}")]
        public async Task<IActionResult> GetPagedMachines(long CompanyId,int amount, int page)
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
                .ThenInclude(mx => mx.Manufacturer)
                .Where(vm => vm.CompanyID == CompanyId)
                .ToListAsync();

            if (machines == null || machines.Count == 0)
            {
                return NotFound();
            }

            int machinesAmount = machines.Count();

            if (Math.Ceiling((double)machines.Count / amount) < page)
            {
                return NotFound("Page does not exist!");
            }

            machines = machines.Skip((page - 1) * amount).Take(amount).ToList();

            var response = new PagedMachinesResult
            {
                VendingMachines = machines,
                TotalCount = machinesAmount
            };

            return Ok(response);
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
        public async Task<IActionResult> PostVendingMachine(VendingMachineCreateDTO machine)
        {
            VendingMachine vendingMachine = new VendingMachine
            {
                ID = machine.ID,
                StatusID = machine.StatusID,
                OperatingModeID = machine.OperatingModeID,
                CompanyID = machine.CompanyID,
                ModelID = machine.ModelID,
                ModemID = machine.ModemID,
                TimeZone = machine.TimeZone,
                Name = machine.Name,
                Adress = machine.Adress,
                Coordinates = machine.Coordinates,
                PlacementType = machine.PlacementType,
                PlacementDate = machine.PlacementDate,
                StartHours = machine.StartHours,
                EndHours = machine.EndHours
            };

            try
            {
                _context.VendingMachines.Add(vendingMachine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsVendingMachineExists(vendingMachine.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostVendingMachine), vendingMachine);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutVendingMachine(long Id, VendingMachineCreateDTO machine)
        {
            if (Id != machine.ID) return BadRequest("Ids do not match!");
            if (!await IsVendingMachineExists(Id)) return NotFound();

            VendingMachine vendingMachine = new VendingMachine
            {
                ID = machine.ID,
                StatusID = machine.StatusID,
                OperatingModeID = machine.OperatingModeID,
                CompanyID = machine.CompanyID,
                ModelID = machine.ModelID,
                ModemID = machine.ModemID,
                TimeZone = machine.TimeZone,
                Name = machine.Name,
                Adress = machine.Adress,
                Coordinates = machine.Coordinates,
                PlacementType = machine.PlacementType,
                PlacementDate = machine.PlacementDate,
                StartHours = machine.StartHours,
                EndHours = machine.EndHours
            };

            try
            {
                _context.Entry(vendingMachine).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(vendingMachine);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteVendingMachine(long Id)
        {
            if (!await IsVendingMachineExists(Id))
                return NotFound();
            try
            {
                var machineProductAvaliab = await _context.VendingAvaliabilities
                    .Where(v => v.VendingMachineID == Id)
                    .ToListAsync();
                _context.VendingAvaliabilities.RemoveRange(machineProductAvaliab);

                var machineMoneyAvaliab = await _context.VendingMachineMoney
                    .Where(v => v.VendingMachineID == Id)
                    .ToListAsync();
                _context.VendingMachineMoney.RemoveRange(machineMoneyAvaliab);

                var machineMaintenances = await _context.Maintenances
                    .Where(m => m.VendingMachineID == Id)
                    .ToListAsync();
                _context.Maintenances.RemoveRange(machineMaintenances);

                var machineSales = await _context.Sales
                    .Where(s => s.VendingMachineID == Id)
                    .ToListAsync();
                _context.Sales.RemoveRange(machineSales);

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
