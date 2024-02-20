using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]//nastavi osnovno pot za klice API-ja za obvestila
    [ApiController]//označuje, da je ta razred kontroler za API
    public class ObvestiloController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObvestiloController(ApplicationDbContext context)
        {
            _context = context;//konstruktor inicializira kontekst za dostop do baze
        }

        // GET: api/Obvestilo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obvestilo>>> GetObvestila()
        {
            //vrne vse zapise obvestil skupaj z njihovimi termini
            return await _context.Obvestila.Include(o => o.Termin).ToListAsync();
        }

        // GET: api/Obvestilo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obvestilo>> GetObvestilo(int id)
        {
            //poišče obvestilo po ID-ju, vključi povezan termin
            var obvestilo = await _context.Obvestila.Include(o => o.Termin).FirstOrDefaultAsync(o => o.Id == id);

            if (obvestilo == null)
            {
                return NotFound();//če obvestilo ni najdeno, vrne 404
            }

            return obvestilo;//vrne najdeno obvestilo
        }

        // POST: api/Obvestilo
        [HttpPost]
        public async Task<ActionResult<Obvestilo>> PostObvestilo(Obvestilo obvestilo)
        {
            //doda novo obvestilo in shrani spremembe
            _context.Obvestila.Add(obvestilo);
            await _context.SaveChangesAsync();

            //vrne ustvarjeno obvestilo
            return CreatedAtAction("GetObvestilo", new { id = obvestilo.Id }, obvestilo);
        }

        // PUT: api/Obvestilo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObvestilo(int id, Obvestilo obvestilo)
        {
            if (id != obvestilo.Id)
            {
                return BadRequest();//preveri usklajenost ID-jev, vrne napako če se ne ujemajo
            }

            _context.Entry(obvestilo).State = EntityState.Modified;//označi obvestilo kot spremenjeno

            try
            {
                await _context.SaveChangesAsync();//poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObvestiloExists(id))
                {
                    return NotFound();//če obvestilo ne obstaja, vrne 404
                }
                else
                {
                    throw;//če pride do drugih napak pri shranjevanju
                }
            }

            return NoContent();//vrne status 204, ko so spremembe uspešno shranjene
        }

        // DELETE: api/Obvestilo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObvestilo(int id)
        {
            //poišče obvestilo po ID-ju
            var obvestilo = await _context.Obvestila.FindAsync(id);
            if (obvestilo == null)
            {
                return NotFound();//če obvestilo ni najdeno, vrne 404
            }

            _context.Obvestila.Remove(obvestilo);//odstrani obvestilo iz baze
            await _context.SaveChangesAsync();//shrani spremembe

            return NoContent();//vrne status 204 po uspešnem brisanju
        }

        private bool ObvestiloExists(int id)
        {
            //preveri, če obvestilo z danim ID-jem obstaja v bazi
            return _context.Obvestila.Any(e => e.Id == id);
        }
    }
}
