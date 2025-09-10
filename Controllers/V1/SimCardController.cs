using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SimCardController : BaseController<SimCard, SimCardDTO, long>
    {
        public SimCardController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(SimCardDTO entity) => entity.ID;

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedResult<SimCardDTO>>> GetPagedSims(int amount, int page, [FromQuery] long CompanyId, [FromQuery] bool linked)
        {
            var query = _context.SimCards.AsQueryable();

            if (CompanyId != 0)
            {
                query = query.Where(c => c.CompanyID == CompanyId);
            }
            if (linked)
            {
                query = query.Where(sim => !_context.Modems.Any(m => m.SimCardID == sim.ID));
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

            var dto = _mapper.Map<List<SimCardDTO>>(items);

            var result = new PagedResult<SimCardDTO>
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
