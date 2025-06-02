using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ManagementDbContext _context;
        public CompanyController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {

            var companies = await _context.Companies
                .Include(c => c.CompanyUsers)
                .Include(c => c.VendingMachines)
                .ToListAsync();

            if (companies == null || !companies.Any())
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(companies);
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetCompany(long id)
        {
            //Company comp = new Company;


            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return BadRequest(new { message = "No data was found" });
            }

            return Ok(company);
        }
    }
}
  