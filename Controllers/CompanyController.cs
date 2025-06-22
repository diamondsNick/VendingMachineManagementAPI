using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ManagementDbContext _context;

        public CompaniesController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedCompanies>> GetPagedCompanies(int amount, int page, [FromQuery] long parentCompanyId)
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

            var result = new PagedCompanies
            {
                Companies = items,
                TotalCount = totalCount
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(long id, Company company)
        {
            if (id != company.ID)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.ID }, company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CompanyExists(long id)
        {
            return await _context.Companies.AnyAsync(e => e.ID == id);
        }
    }
}