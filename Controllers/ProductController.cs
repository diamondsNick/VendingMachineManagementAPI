using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ManagementDbContext _context;
        public ProductController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {

            var products = await _context.Products.ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProduct(long Id)
        {
            var products = await _context.Products.FindAsync(Id);

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsProductExists(product.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostProduct), product);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutProduct(long Id, Product product)
        {
            if (Id != product.ID) return BadRequest("Ids does not match!");
            if (!await IsProductExists(Id)) return NotFound();
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(product);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteProduct(long Id)
        {
            if (!await IsProductExists(Id))
                return NotFound();
            try
            {
                var product = await _context.Products.FindAsync(Id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsProductExists(long Id)
        {
            bool exists = await _context.Products.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
