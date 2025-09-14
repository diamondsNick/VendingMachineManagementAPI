using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ManufacturerController : BaseController<Manufacturer, ManufacturerDTO, long>
    {
        public ManufacturerController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(ManufacturerDTO entity) => entity.ID;

        [HttpGet]
        public async override Task<ActionResult> GetEntities()
        {
            if (!await _context.Manufacturers.AnyAsync()) return NotFound();
            try
            {
                var manufacturers = await _context.Manufacturers
                    .Include(e => e.VendingMachineMatrices)
                    .ToArrayAsync();
                return Ok(manufacturers);
            }
            catch (DBConcurrencyException) { throw; }
        }

        [HttpGet("{Id}")]
        public override async Task<ActionResult> GetByID([FromRoute] long ID)
        {
            if (!await IsManufacturerExists(ID)) return NotFound();
            try
            {
                var manufacturers = await _context.Manufacturers
                    .Include(e => e.VendingMachineMatrices)
                    .FirstAsync(e => e.ID == ID);
                return Ok(manufacturers);
            }
            catch (DBConcurrencyException) { throw; }
        }
        
        private async Task<bool> IsManufacturerExists(long Id)
        {
            var exists = await _context.Manufacturers.AnyAsync(e => e.ID == Id);
            return exists;
        }
    }
}
