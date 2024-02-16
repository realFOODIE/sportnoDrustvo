using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RekvizitController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RekvizitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rekvizit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rekvizit>>> GetRekviziti()
        {
            return await _context.Rekviziti.Include(r => r.Clan).ToListAsync();
        }

        // GET: api/Rekvizit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rekvizit>> GetRekvizit(int id)
        {
            var rekvizit = await _context.Rekviziti.Include(r => r.Clan).FirstOrDefaultAsync(r => r.Id == id);

            if (rekvizit == null)
            {
                return NotFound();
            }

            return rekvizit;
        }

        // POST: api/Rekvizit
        [HttpPost]
        public async Task<ActionResult<Rekvizit>> PostRekvizit(Rekvizit rekvizit)
        {
            _context.Rekviziti.Add(rekvizit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRekvizit), new { id = rekvizit.Id }, rekvizit);
        }

        // PUT: api/Rekvizit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRekvizit(int id, Rekvizit rekvizit)
        {
            if (id != rekvizit.Id)
            {
                return BadRequest();
            }

            _context.Entry(rekvizit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RekvizitExists(id))
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

        // DELETE: api/Rekvizit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRekvizit(int id)
        {
            var rekvizit = await _context.Rekviziti.FindAsync(id);
            if (rekvizit == null)
            {
                return NotFound();
            }

            _context.Rekviziti.Remove(rekvizit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RekvizitExists(int id)
        {
            return _context.Rekviziti.Any(e => e.Id == id);
        }
    }
}
