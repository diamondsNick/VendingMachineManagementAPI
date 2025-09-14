using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleController : BaseController<Sale, SaleDTO, long>
    {
        public SaleController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(SaleDTO entity) => entity.ID;

        [HttpGet("company/{CompanyId}")]
        public async Task<IActionResult> GetCompanySales(long CompanyId)
        {
            var sale = await _context.Sales
                .Include(s => s.VendingMachine)
                .Where(s => s.VendingMachine.CompanyID == CompanyId)
                .ToListAsync();

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }
    }
}
