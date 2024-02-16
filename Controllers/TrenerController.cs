using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrenerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrenerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Trener
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trener>>> GetTrenerji()
        {
            return await _context.Trenerji.ToListAsync();
        }

        // GET: api/Trener/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trener>> GetTrener(int id)
        {
            var trener = await _context.Trenerji.FindAsync(id);

            if (trener == null)
            {
                return NotFound();
            }

            return trener;
        }

        // POST: api/Trener
        [HttpPost]
        public async Task<ActionResult<Trener>> PostTrener(Trener trener)
        {
            _context.Trenerji.Add(trener);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrener), new { id = trener.Id }, trener);
        }

        // PUT: api/Trener/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrener(int id, Trener trener)
        {
            if (id != trener.Id)
            {
                return BadRequest();
            }

            _context.Entry(trener).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrenerExists(id))
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

        // DELETE: api/Trener/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrener(int id)
        {
            var trener = await _context.Trenerji.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }

            _context.Trenerji.Remove(trener);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrenerExists(int id)
        {
            return _context.Trenerji.Any(e => e.Id == id);
        }
    }
}
