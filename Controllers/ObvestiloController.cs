using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObvestiloController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObvestiloController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Obvestilo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obvestilo>>> GetObvestila()
        {
            return await _context.Obvestila.Include(o => o.Termin).ToListAsync();
        }

        // GET: api/Obvestilo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obvestilo>> GetObvestilo(int id)
        {
            var obvestilo = await _context.Obvestila.Include(o => o.Termin).FirstOrDefaultAsync(o => o.Id == id);

            if (obvestilo == null)
            {
                return NotFound();
            }

            return obvestilo;
        }

        // POST: api/Obvestilo
        [HttpPost]
        public async Task<ActionResult<Obvestilo>> PostObvestilo(Obvestilo obvestilo)
        {
            _context.Obvestila.Add(obvestilo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObvestilo", new { id = obvestilo.Id }, obvestilo);
        }

        // PUT: api/Obvestilo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObvestilo(int id, Obvestilo obvestilo)
        {
            if (id != obvestilo.Id)
            {
                return BadRequest();
            }

            _context.Entry(obvestilo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObvestiloExists(id))
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

        // DELETE: api/Obvestilo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObvestilo(int id)
        {
            var obvestilo = await _context.Obvestila.FindAsync(id);
            if (obvestilo == null)
            {
                return NotFound();
            }

            _context.Obvestila.Remove(obvestilo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObvestiloExists(int id)
        {
            return _context.Obvestila.Any(e => e.Id == id);
        }
    }
}
