using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO;
using VendingMachineManagementAPI.DTOs.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VendingMachineController : BaseController<VendingMachine, VendingMachineDTO, long>
    {
        public VendingMachineController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(VendingMachineDTO entity) => entity.ID;

        [HttpGet]
        public override async Task<ActionResult> GetEntities()
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
        public async Task<IActionResult> GetPagedMachines(long CompanyId, int amount, int page)
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

            var response = new PagedResult<VendingMachine>
            {
                Items = machines,
                TotalAmount = machinesAmount,
                TotalPages = machinesAmount / amount,
                Page = page,
                PageAmount = amount
            };

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public override async Task<ActionResult> GetByID(long Id)
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

        [HttpDelete("{Id}")]
        public override async Task<ActionResult> DeleteEntity(long Id)
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
