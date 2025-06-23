using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VendingMachineManagementAPI.Data;
using VendingMachineManagementAPI.DTOs;
using VendingMachineManagementAPI.Models;

namespace VendingMachineManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModemController : Controller
    {
        private readonly ManagementDbContext _context;
        public ModemController(ManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetModems()
        {

            var modems = await _context.Modems.ToListAsync();

            if (modems == null || !modems.Any())
            {
                return NotFound();
            }

            return Ok(modems);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetModem(long Id)
        {
            var modem = await _context.Modems.FindAsync(Id);

            if (modem == null)
            {
                return NotFound();
            }

            return Ok(modem);
        }

        [HttpGet("{amount:int}/{page:int}")]
        public async Task<ActionResult<PagedModems>> GetPagedCompanies(int amount, int page, [FromQuery] long CompanyId)
        {
            var query = _context.Modems.AsQueryable();

            if (CompanyId != 0)
            {
                query = query.Where(c => c.CompanyID == CompanyId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.ID)
                .Skip((page - 1) * amount)
                .Take(amount)
            .ToListAsync();

            if (items == null || items.Count == 0)
            {
                return NotFound("Page does not exist!");
            }

            var result = new PagedModems
            {
                Modems = items,
                TotalCount = totalCount
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostModem(ModemCreateDTO modemInfo)
        {
            Modem modem = new()
            {
                ID = modemInfo.ID,
                Model = modemInfo.Model,
                SimCardID = modemInfo.SimCardID,
                SerialNum = modemInfo.SerialNum,
                Password = modemInfo.Password,
                CompanyID = modemInfo.CompanyID
            };
            try
            {
                _context.Modems.Add(modem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IsModemExists(modem.ID))
                    return BadRequest("Already Exists!");
                else throw;
            }
            return CreatedAtAction(nameof(PostModem), modem);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutModem(long Id, ModemCreateDTO modemInfo)
        {
            Modem modem = new()
            {
                ID = modemInfo.ID,
                Model = modemInfo.Model,
                SimCardID = modemInfo.SimCardID,
                SerialNum = modemInfo.SerialNum,
                Password = modemInfo.Password,
                CompanyID = modemInfo.CompanyID
            };
            if (Id != modem.ID) return BadRequest("Ids does not match!");
            if (!await IsModemExists(Id)) return NotFound();
            try
            {
                _context.Entry(modem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteModem(long Id)
        {
            if (!await IsModemExists(Id))
                return NotFound();
            try
            {
                var modem = await _context.Modems.FindAsync(Id);
                _context.Modems.Remove(modem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }
            return Ok();
        }

        private async Task<bool> IsModemExists(long Id)
        {
            bool exists = await _context.Modems.AnyAsync(m => m.ID == Id);
            return exists;
        }
    }
}
