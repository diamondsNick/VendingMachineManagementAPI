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
    public class VendingMachineMatrixController : Controller
    {
        private readonly ManagementDbContext _context;
        public VendingMachineMatrixController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVendingMachineMatrices()
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
        public async Task<IActionResult> GetVendingMachineMatrix(long Id)
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

        [HttpPost]
        public async Task<IActionResult> PostVendingMachineMatrix(VendingMachineMatrix matrix)
        {
            try
            {
                _context.VendingMachineMatrices.Add(matrix);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsVendingMachineMatrixExists(matrix.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostVendingMachineMatrix), matrix);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutVendingMachineMatrix(long Id, VendingMachineMatrix matrix)
        {
            if (Id != matrix.ID) return BadRequest("Ids do not match!");
            if (!await IsVendingMachineMatrixExists(Id)) return NotFound();
            try
            {
                _context.Entry(matrix).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(matrix);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteVendingMachineMatrix(long Id)
        {
            if (!await IsVendingMachineMatrixExists(Id))
                return NotFound();
            try
            {
                var matrix = await _context.VendingMachineMatrices.FindAsync(Id);
                _context.VendingMachineMatrices.Remove(matrix);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsVendingMachineMatrixExists(long Id)
        {
            bool exists = await _context.VendingMachineMatrices.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
