using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using VendingMachineManagementAPI.Data;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly ManagementDbContext _context;
        public AuthentificationController(ManagementDbContext context)
        {
            _context = context;
        }
        [HttpGet("{Login}/{Password}")]
        public async Task<IActionResult> AuthentificateUser(string Login, string Password)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Login == Login && x.Password == Password);
            if (user == null)
            {
                return NotFound();
            }
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }
    }
}
