using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO.V1;

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

        [HttpPost]
        public async Task<IActionResult> AuthentificateUser([FromBody] UserAuthDTO loginData)
        {
            var user = await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Login == loginData.Login && x.Password == loginData.Password);

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
