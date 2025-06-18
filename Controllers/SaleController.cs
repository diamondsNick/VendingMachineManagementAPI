using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : Controller
    {
        private readonly ManagementDbContext _context;
        public SaleController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var sales = await _context.Sales.ToListAsync();

            if (sales == null || !sales.Any())
            {
                return NotFound();
            }

            return Ok(sales);
        }

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
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSale(long Id)
        {
            var sale = await _context.Sales.FindAsync(Id);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> PostSale(Sale sale)
        {
            try
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsSaleExists(sale.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostSale), sale);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutSale(long Id, Sale sale)
        {
            if (Id != sale.ID) return BadRequest("Ids does not match!");
            if (!await IsSaleExists(Id)) return NotFound();
            try
            {
                _context.Entry(sale).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(sale);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteSale(long Id)
        {
            if (!await IsSaleExists(Id))
                return NotFound();
            try
            {
                var sale = await _context.Sales.FindAsync(Id);
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsSaleExists(long Id)
        {
            bool exists = await _context.Sales.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
