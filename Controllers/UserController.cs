using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs;
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
        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedUsers>> GetPagedUsers(int amount, int page, [FromQuery] long CompanyId)
        {
            var query = _context.Users.AsQueryable();

            if (CompanyId != 0)
            {
                query = query.Where(c => c.CompanyID == CompanyId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(u => u.Company)
                .Include(u => u.Role)
                .OrderBy(c => c.ID)
                .Skip((page - 1) * amount)
                .Take(amount)
            .ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound("Page does not exist!");
            }

            var result = new PagedUsers
            {
                Users = items,
                TotalCount = totalCount
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser(PagedUsersCreateDTO userDTO)
        {
            User user = new User();
            try
            {
                user = new()
                {
                    ID = (long)userDTO.ID,
                    FullName = userDTO.FullName,
                    Email = userDTO.Email,
                    Phone = userDTO.Phone,
                    RegistrationDate = userDTO.RegistrationDate,
                    RoleID = userDTO.RoleID,
                    CompanyID = userDTO.CompanyID,
                    Language = userDTO.Language,
                    Login = userDTO.Login,
                    Password = userDTO.Password
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await UserExists((long)userDTO.ID))
                    return BadRequest("User already exists!");
                else throw;
            }

            return CreatedAtAction(nameof(GetUserInfo), new { Id = user.ID }, user);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutUser(long Id, PagedUsersCreateDTO userDTO)
        {
            User user = new User()
            {
                ID = (long)userDTO.ID,
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                RegistrationDate = userDTO.RegistrationDate,
                RoleID = userDTO.RoleID,
                CompanyID = userDTO.CompanyID,
                Language = userDTO.Language,
                Login = userDTO.Login,
                Password = userDTO.Password
            };

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
