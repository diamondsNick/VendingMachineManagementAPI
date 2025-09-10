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
    public class CompaniesController : BaseController<Company, CompanyDTO, long>
    {
        public CompaniesController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(CompanyDTO entity) => entity.ID;

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<CompanyDTO>> GetPagedCompanies(int amount, int page, [FromQuery] long parentCompanyId)
        {
            var query = _context.Companies.AsQueryable();

            if (parentCompanyId != 0)
            {
                query = query.Where(c => c.ParentCompanyID == parentCompanyId);
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

            var dto = _mapper.Map<List<CompanyDTO>>(items);

            var result = new PagedResult<CompanyDTO>
            {
                Items = dto,
                TotalAmount = totalCount,
                Page = page,
                TotalPages = totalCount / amount,
                PageAmount = page
            };

            return Ok(result);
        }
    }
}