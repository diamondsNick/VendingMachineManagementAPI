using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TDTO, TKey> : ControllerBase
        where TEntity : class
    {
        private readonly ManagementDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IMapper _mapper;

        public BaseController(ManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<TDTO>> GetEntities()
        {
            var res = await _dbSet.ToListAsync();

            if (!res.Any()) return NotFound();

            var mapped = _mapper.Map<List<TEntity>>(res);

            return Ok(mapped);
        }

        [HttpPut]
        public virtual async Task<ActionResult<TEntity>> PutEntity([FromBody] TDTO)
        {
            if
        }

    }
}
