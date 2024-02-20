using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]//nastavi osnovno pot za klice API-ja za rezervacije
    [ApiController]//označuje, da je ta razred kontroler za API
    public class RezervacijaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;//konstruktor inicializira kontekst za dostop do baze
        }

        // GET: api/Rezervacija
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezervacija>>> GetRezervacije()
        {
            //vrne vse rezervacije skupaj z njihovimi termini in člani
            return await _context.Rezervacije
                                 .Include(r => r.Termin)
                                 .Include(r => r.Clan)
                                 .ToListAsync();
        }

        // GET: api/Rezervacija/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rezervacija>> GetRezervacija(int id)
        {
            //poišče rezervacijo po ID-ju, vključi povezane termine in člane
            var rezervacija = await _context.Rezervacije
                                            .Include(r => r.Termin)
                                            .Include(r => r.Clan)
                                            .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
            {
                return NotFound();//če rezervacija ni najdena, vrne 404
            }

            return rezervacija;//vrne najdeno rezervacijo
        }

        // POST: api/Rezervacija
        [HttpPost]
        public async Task<ActionResult<Rezervacija>> PostRezervacija(Rezervacija rezervacija)
        {
            //doda novo rezervacijo in shrani spremembe
            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            //vrne ustvarjeno rezervacijo
            return CreatedAtAction("GetRezervacija", new { id = rezervacija.Id }, rezervacija);
        }

        // PUT: api/Rezervacija/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervacija(int id, Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return BadRequest();//preveri usklajenost ID-jev, vrne napako če se ne ujemajo
            }

            _context.Entry(rezervacija).State = EntityState.Modified;//označi rezervacijo kot spremenjeno

            try
            {
                await _context.SaveChangesAsync();//poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervacijaExists(id))
                {
                    return NotFound();//če rezervacija ne obstaja, vrne 404
                }
                else
                {
                    throw;//če pride do drugih napak pri shranjevanju
                }
            }

            return NoContent();//vrne status 204, ko so spremembe uspešno shranjene
        }

        // DELETE: api/Rezervacija/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervacija(int id)
        {
            //poišče rezervacijo po ID-ju
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();//če rezervacija ni najdena, vrne 404
            }

            _context.Rezervacije.Remove(rezervacija);//odstrani rezervacijo iz baze
            await _context.SaveChangesAsync();//shrani spremembe

            return NoContent();//vrne status 204 po uspešnem brisanju
        }

        private bool RezervacijaExists(int id)
        {
            //preveri, če rezervacija z danim ID-jem obstaja v bazi
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}
