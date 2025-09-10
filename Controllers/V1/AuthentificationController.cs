using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
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
