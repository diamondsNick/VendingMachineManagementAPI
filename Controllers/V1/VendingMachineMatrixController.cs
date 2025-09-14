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
    public class VendingMachineMatrixController : BaseController<VendingMachineMatrix, VendingMachineMatrixDTO, long>
    {
        public VendingMachineMatrixController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(VendingMachineMatrixDTO entity) => entity.ID;

        [HttpGet]
        public override async Task<ActionResult> GetEntities()
        {
            var matrices = await _context.VendingMachineMatrices
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VendingMachines)
                .ToListAsync();

            if (matrices == null || !matrices.Any())
            {
                return NotFound();
            }

            return Ok(matrices);
        }

        [HttpGet("{Id}")]
        public async override Task<ActionResult> GetByID(long Id)
        {
            var matrix = await _context.VendingMachineMatrices
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VendingMachines)
                .FirstOrDefaultAsync(vm => vm.ID == Id);

            if (matrix == null)
            {
                return NotFound();
            }

            return Ok(matrix);
        }
    }
}
