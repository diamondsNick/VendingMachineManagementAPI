using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TDTO, TKey> : ControllerBase
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

        [HttpPost]
        public virtual async Task<ActionResult<TDTO>> PostEntity([FromBody] TDTO entity)
        {
            if (entity == null) return BadRequest("Body value cannot be null");

            try
            {
                var res = _mapper.Map<TEntity>(entity);

                await _dbSet.AddAsync(res);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostEntity), res);
            }
            catch (DbUpdateException)
            {
                return Problem("Internal DB error", statusCode: 500);
            }
            catch (Exception)
            {
                return Problem("Internal server error", statusCode: 500);
            }

        }

        [HttpPut("{ID}")]
        public virtual async Task<ActionResult<TDTO>> PutEntity([FromRoute] TKey ID, [FromBody] TDTO entity)
        {
            if (entity == null) return BadRequest("Request body is empty");

            if (!ID.Equals(GetKey(entity))) return BadRequest("IDs does not match");

            var storedEntity = await _dbSet.FindAsync(ID);

            if (storedEntity == null) return NotFound("Object was not found");

            try
            {
                _mapper.Map(entity, storedEntity);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return Problem("Internal server error", statusCode: 500);
            }
        }

        [HttpDelete("{ID}")]
        public virtual async Task<ActionResult> DeleteEntity([FromRoute] TKey ID)
        {
            if (ID == null) return BadRequest("ID value cannot be null");

            var deleted = await _dbSet.FindAsync(ID);

            if (deleted == null) return NotFound("Object was not found");

            try
            {
                _dbSet.Remove(deleted);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Problem("Internal DB error", statusCode: 500);
            }
            catch (Exception)
            {
                return Problem("Internal server error", statusCode: 500);
            }
        }

        protected abstract TKey GetKey(TDTO entity);

    }
}
