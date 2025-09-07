using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController <TEntity, TDTO, TKey> : ControllerBase
        where TEntity: class
    {
        private readonly ManagementDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseController(ManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        [HttpGet]
        public virtual async Task<ActionResult<TDTO>> GetAll()
        {
            var res = await _dbSet.ToListAsync();

            if (!res.Any()) return NotFound;


        }


    }
}
