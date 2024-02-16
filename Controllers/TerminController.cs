using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Termin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Termin>>> GetTermini()
        {
            return await _context.Termini.ToListAsync();
        }

        // GET: api/Termin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Termin>> GetTermin(int id)
        {
            var termin = await _context.Termini.FindAsync(id);

            if (termin == null)
            {
                return NotFound();
            }

            return termin;
        }

        // POST: api/Termin
        [HttpPost]
        public async Task<ActionResult<Termin>> PostTermin(Termin termin)
        {
            _context.Termini.Add(termin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTermin", new { id = termin.Id }, termin);
        }

        // PUT: api/Termin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTermin(int id, Termin termin)
        {
            if (id != termin.Id)
            {
                return BadRequest();
            }

            _context.Entry(termin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerminExists(id))
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

        // DELETE: api/Termin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTermin(int id)
        {
            var termin = await _context.Termini.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }

            _context.Termini.Remove(termin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TerminExists(int id)
        {
            return _context.Termini.Any(e => e.Id == id);
        }
    }
}
