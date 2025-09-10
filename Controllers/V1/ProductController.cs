using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : BaseController<Product, ProductDTO, long>
    {

        public ProductController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(ProductDTO entity) => entity.ID;

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetProducts([FromQuery] int amount = 0, [FromQuery] int page = 0)
        {

            var products = _context.Products.AsQueryable();

            if (products == null || !products.Any())
            {
                return NotFound();
            }
            var totalCount = await products.CountAsync();

            //if (page != 0 && amount != 0)
            //{
            //    var res = await products
            //    .OrderBy(p => p.ID)
            //    .Skip((page - 1) * amount)
            //    .Take(amount)
            //    .ToListAsync();
            //}
            //else
            //{
            //    var res = await products.ToListAsync();
            //}

            var res = await products
                .OrderBy(p => p.ID)
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToListAsync();

            var dto = _mapper.Map<List<ProductDTO>>(res);

            var result = new PagedResult<ProductDTO>
            {
                Items = dto,
                Page = page,
                TotalAmount = totalCount,
                TotalPages = totalCount / amount,
                PageAmount = amount
            };

            return Ok(result);
        }
    }
}
