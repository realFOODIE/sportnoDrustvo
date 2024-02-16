using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervacijaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rezervacija
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezervacija>>> GetRezervacije()
        {
            return await _context.Rezervacije
                                 .Include(r => r.Termin)
                                 .Include(r => r.Clan)
                                 .ToListAsync();
        }

        // GET: api/Rezervacija/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rezervacija>> GetRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije
                                            .Include(r => r.Termin)
                                            .Include(r => r.Clan)
                                            .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return rezervacija;
        }

        // POST: api/Rezervacija
        [HttpPost]
        public async Task<ActionResult<Rezervacija>> PostRezervacija(Rezervacija rezervacija)
        {
            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRezervacija", new { id = rezervacija.Id }, rezervacija);
        }

        // PUT: api/Rezervacija/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervacija(int id, Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return BadRequest();
            }

            _context.Entry(rezervacija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervacijaExists(id))
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

        // DELETE: api/Rezervacija/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}
