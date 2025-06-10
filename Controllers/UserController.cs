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
            if (!await _context.Users.AnyAsync()) return NotFound("No users in DB!");
            var users = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .ToListAsync();
            return Ok(users);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserInfo(long Id)
        {
            if (!await UserExists(Id)) return NotFound();
            try
            {
                var user = await _context.Users
                    .Include(u => u.Company)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.ID == Id);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        
        private async Task<bool> UserExists(long Id)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.ID == Id);
            return userExists;
        }
    }
}
