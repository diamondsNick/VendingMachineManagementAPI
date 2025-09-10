using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    public class ModemController : BaseController<Modem, ModemDTO, long>
    {
        public ModemController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(ModemDTO entity) => entity.ID;

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedResult<ModemDTO>>> GetPagedCompanies(int amount, int page, [FromQuery] long CompanyId)
        {
            var query = _context.Modems.AsQueryable();

            if (CompanyId != 0)
            {
                query = query.Where(c => c.CompanyID == CompanyId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.ID)
                .Skip((page - 1) * amount)
                .Take(amount)
            .ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound("Page does not exist!");
            }

            var dto = _mapper.Map<List<ModemDTO>>(items);

            var result = new PagedResult<ModemDTO>
            {
                Items = dto,
                TotalAmount = totalCount,
                TotalPages = totalCount / amount,
                Page = page,
                PageAmount = amount
            };

            return Ok(result);
        }
    }
}
