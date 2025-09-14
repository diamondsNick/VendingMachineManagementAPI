using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTO.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatusController : BaseController<Status, StatusDTO, long>
    {
        public StatusController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(StatusDTO entity) => entity.ID;
    }
}
