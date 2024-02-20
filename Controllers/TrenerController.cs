using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]//nastavi osnovno pot za klice API-ja za trenerje
    [ApiController]//označuje, da je ta razred kontroler za API
    public class TrenerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrenerController(ApplicationDbContext context)
        {
            _context = context;//konstruktor inicializira kontekst za dostop do baze
        }

        // GET: api/Trener
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trener>>> GetTrenerji()
        {
            //vrne vse trenerje
            return await _context.Trenerji.ToListAsync();
        }

        // GET: api/Trener/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trener>> GetTrener(int id)
        {
            //poišče trenerja po ID-ju
            var trener = await _context.Trenerji.FindAsync(id);

            if (trener == null)
            {
                return NotFound();//če trener ni najden, vrne 404
            }

            return trener;//vrne najdenega trenerja
        }

        // POST: api/Trener
        [HttpPost]
        public async Task<ActionResult<Trener>> PostTrener(Trener trener)
        {
            //doda novega trenerja in shrani spremembe
            _context.Trenerji.Add(trener);
            await _context.SaveChangesAsync();

            //vrne ustvarjenega trenerja
            return CreatedAtAction(nameof(GetTrener), new { id = trener.Id }, trener);
        }

        // PUT: api/Trener/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrener(int id, Trener trener)
        {
            if (id != trener.Id)
            {
                return BadRequest();//preveri usklajenost ID-jev, vrne napako če se ne ujemajo
            }

            _context.Entry(trener).State = EntityState.Modified;//označi trenerja kot spremenjenega

            try
            {
                await _context.SaveChangesAsync();//poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrenerExists(id))
                {
                    return NotFound();//če trener ne obstaja, vrne 404
                }
                else
                {
                    throw;//če pride do drugih napak pri shranjevanju
                }
            }

            return NoContent();//vrne status 204, ko so spremembe uspešno shranjene
        }

        // DELETE: api/Trener/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrener(int id)
        {
            //poišče trenerja po ID-ju
            var trener = await _context.Trenerji.FindAsync(id);
            if (trener == null)
            {
                return NotFound();//če trener ni najden, vrne 404
            }

            _context.Trenerji.Remove(trener);//odstrani trenerja iz baze
            await _context.SaveChangesAsync();//shrani spremembe

            return NoContent();//vrne status 204 po uspešnem brisanju
        }

        private bool TrenerExists(int id)
        {
            //preveri, če trener z danim ID-jem obstaja v bazi
            return _context.Trenerji.Any(e => e.Id == id);
        }
    }
}
