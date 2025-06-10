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
    public class RoleController : Controller
    {
        private readonly ManagementDbContext _context;
        public RoleController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if (roles == null || !roles.Any())
            {
                return NotFound();
            }

            return Ok(roles);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRole(long Id)
        {
            var role = await _context.Roles.FindAsync(Id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(Role role)
        {
            try
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsRoleExists(role.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostRole), role);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutRole(long Id, Role role)
        {
            if (Id != role.ID) return BadRequest("Ids does not match!");
            if (!await IsRoleExists(Id)) return NotFound();
            try
            {
                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok(role);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteRole(long Id)
        {
            if (!await IsRoleExists(Id))
                return NotFound();
            try
            {
                var role = await _context.Roles.FindAsync(Id);
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsRoleExists(long Id)
        {
            bool exists = await _context.Roles.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
