using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]//nastavi osnovno pot za klice API-ja za termine
    [ApiController]//označuje, da je ta razred kontroler za API
    public class TerminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;//konstruktor inicializira kontekst za dostop do baze
        }

        // GET: api/Termin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Termin>>> GetTermini()
        {
            //vrne vse termine
            return await _context.Termini.ToListAsync();
        }

        // GET: api/Termin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Termin>> GetTermin(int id)
        {
            //poišče termin po ID-ju
            var termin = await _context.Termini.FindAsync(id);

            if (termin == null)
            {
                return NotFound();//če termin ni najden, vrne 404
            }

            return termin;//vrne najdeni termin
        }

        // POST: api/Termin
        [HttpPost]
        public async Task<ActionResult<Termin>> PostTermin(Termin termin)
        {
            //doda nov termin in shrani spremembe
            _context.Termini.Add(termin);
            await _context.SaveChangesAsync();

            //vrne ustvarjeni termin
            return CreatedAtAction("GetTermin", new { id = termin.Id }, termin);
        }

        // PUT: api/Termin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTermin(int id, Termin termin)
        {
            if (id != termin.Id)
            {
                return BadRequest();//preveri usklajenost ID-jev, vrne napako če se ne ujemajo
            }

            _context.Entry(termin).State = EntityState.Modified;//označi termin kot spremenjen

            try
            {
                await _context.SaveChangesAsync();//poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TerminExists(id))
                {
                    return NotFound();//če termin ne obstaja, vrne 404
                }
                else
                {
                    throw;//če pride do drugih napak pri shranjevanju
                }
            }

            return NoContent();//vrne status 204, ko so spremembe uspešno shranjene
        }

        // DELETE: api/Termin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTermin(int id)
        {
            //poišče termin po ID-ju
            var termin = await _context.Termini.FindAsync(id);
            if (termin == null)
            {
                return NotFound();//če termin ni najden, vrne 404
            }

            _context.Termini.Remove(termin);//odstrani termin iz baze
            await _context.SaveChangesAsync();//shrani spremembe

            return NoContent();//vrne status 204 po uspešnem brisanju
        }

        private bool TerminExists(int id)
        {
            //preveri, če termin z danim ID-jem obstaja v bazi
            return _context.Termini.Any(e => e.Id == id);
        }
    }
}
