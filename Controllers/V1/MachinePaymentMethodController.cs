using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs.V1;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MachinePaymentMethodController : BaseController<MachinePaymentMethod, MachinePaymentMethodDTO, long>
    {
        public MachinePaymentMethodController(ManagementDbContext context, IMapper mapper) : base(context, mapper) { }

        protected override long GetKey(MachinePaymentMethodDTO entity) =>  entity.VendingMachineID;

        [NonAction]
        public override Task<ActionResult<MachinePaymentMethodDTO>> PutEntity([FromRoute] long ID, [FromBody] MachinePaymentMethodDTO entity)
        {
            return Task.FromResult<ActionResult<MachinePaymentMethodDTO>>(BadRequest());
        }

        [NonAction]
        public override Task<ActionResult> DeleteEntity([FromRoute] long ID)
        {
            return Task.FromResult<ActionResult>(BadRequest());
        }

        [HttpDelete("{VendingMachineID}/{PaymentMethodID}")]
        public async Task<ActionResult> DeleteMachinePaymentMethod(long VendingMachineID, long PaymentMethodID)
        {
            if (!await IsMethodExists(VendingMachineID, PaymentMethodID))
                return NotFound();
            var metod = await _context.MachinePaymentMethods.FirstOrDefaultAsync(e => e.VendingMachineID == VendingMachineID && e.PaymentMethodID == PaymentMethodID);
            try
            {
                _context.MachinePaymentMethods.Remove(metod);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsMethodExists(long VendingMachineID, long PaymentMethodID)
        {
            bool method = await _context.MachinePaymentMethods.AnyAsync(m => m.VendingMachineID == VendingMachineID && m.PaymentMethodID == PaymentMethodID);
            return method;
        }
    }
}
