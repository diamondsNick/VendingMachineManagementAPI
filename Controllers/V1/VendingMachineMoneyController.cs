using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VendingMachineMoneyController : BaseController<VendingMachineMoney, VendingMachineMoneyDTO, long>
    {
        public VendingMachineMoneyController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(VendingMachineMoneyDTO entity) => entity.ID;

        [HttpGet]
        public override async Task<ActionResult> GetEntities()
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
        public override async Task<ActionResult> GetByID(long Id)
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
    }
}
