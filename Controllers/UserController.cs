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
    public class UserController : Controller
    {
        private readonly ManagementDbContext _context;

        public UserController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users in DB!");
            }

            return Ok(users);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserInfo(long Id)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.ID == Id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists(user.ID))
                    return BadRequest("User already exists!");
                else throw;
            }

            return CreatedAtAction(nameof(GetUserInfo), new { Id = user.ID }, user);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutUser(long Id, User user)
        {
            if (Id != user.ID) return BadRequest("Ids do not match!");
            if (!await UserExists(Id)) return NotFound();

            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(user);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            var user = await _context.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        private async Task<bool> UserExists(long Id)
        {
            return await _context.Users.AnyAsync(u => u.ID == Id);
        }
    }
}
