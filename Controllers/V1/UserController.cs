using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO;
using VendingMachineManagementAPI.DTOs.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : BaseController<User, UserDTO, long>
    {
        public UserController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(UserDTO entity) => entity.ID;

        [HttpGet]
        public override async Task<ActionResult> GetEntities()
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
        public override async Task<ActionResult> GetByID([FromRoute] long Id)
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
        public async Task<ActionResult<PagedResult<User>>> GetPagedUsers(int amount, int page, [FromQuery] long CompanyId)
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

            var dto = _mapper.Map<List<UserDTO>>(items);

            var result = new PagedResult<UserDTO>
            {
                Items = dto,
                TotalAmount = totalCount,
                TotalPages = totalCount / amount,
                PageAmount = amount,
                Page = page
            };

            return Ok(result);
        }
    }
}
