using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceManagement.Server.Data;
using FinanceManagement.Shared;

namespace FinanceManagement.Server.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class EarningsController : ControllerBase
    {
        private readonly FinanceDbContext _context;
        private readonly IConfiguration Configuration;

        public EarningsController(FinanceDbContext context, IConfiguration configuration)
        {
            _context = context;
            this.Configuration = configuration;
        }

        // GET: api/Earnings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Earning>>> GetEarnings(string sortOrder = "", string? searchString = null)
        {

            IQueryable<Earning> earningsIQ = from e in _context.Earnings
                                             select e;

            if (_context.Earnings == null)
            {
                return NotFound();
            }

            switch (sortOrder)
            {
                case "category":
                    earningsIQ = earningsIQ.OrderBy(e => e.Category);
                    break;
                case "subject":
                    earningsIQ = earningsIQ.OrderBy(e => e.Subject);
                    break;
                case "date_desc":
                    earningsIQ = earningsIQ.OrderBy(e => e.Date);
                    break;
                default:
                    earningsIQ = earningsIQ.OrderBy(e => e.Date);
                    break;
            }

            return await earningsIQ.AsNoTracking().ToListAsync();

            //return await _context.Earnings.AsNoTracking().OrderBy(e => e.Date).ToListAsync();
        }

        // GET: api/Earnings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Earning>> GetEarning(int id)
        {
            if (_context.Earnings == null)
            {
                return NotFound();
            }
            //var earning = await _context.Earnings.FindAsync(id);
            var earning = await _context.Earnings.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            if (earning == null)
            {
                return NotFound();
            }

            return earning;
        }

        // PUT: api/Earnings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEarning(int id, Earning earning)
        {
            if (id != earning.Id)
            {
                return BadRequest();
            }

            _context.Entry(earning).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EarningExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Earnings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Earning>> PostEarning(Earning earning)
        {
            if (_context.Earnings == null)
            {
                return Problem("Entity set 'EarningDbContext.Earnings'  is null.");
            }

            _context.Earnings.Add(earning);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEarning), new { id = earning.Id }, earning);
        }

        // DELETE: api/Earnings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEarning(int id)
        {
            if (_context.Earnings == null)
            {
                return NotFound();
            }
            var earning = await _context.Earnings.FindAsync(id);
            if (earning == null)
            {
                return NotFound();
            }

            _context.Earnings.Remove(earning);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EarningExists(int id)
        {
            return (_context.Earnings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
