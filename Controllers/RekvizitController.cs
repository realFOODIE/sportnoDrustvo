using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]//nastavi osnovno pot za klice API-ja za rekvizite
    [ApiController]//označuje, da je ta razred kontroler za API
    public class RekvizitController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RekvizitController(ApplicationDbContext context)
        {
            _context = context;//konstruktor inicializira kontekst za dostop do baze
        }

        // GET: api/Rekvizit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rekvizit>>> GetRekviziti()
        {
            //vrne vse zapise rekvizitov skupaj z njihovimi lastniki
            return await _context.Rekviziti.Include(r => r.Clan).ToListAsync();
        }

        // GET: api/Rekvizit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rekvizit>> GetRekvizit(int id)
        {
            //poišče rekvizit po ID-ju, vključi povezanega člana
            var rekvizit = await _context.Rekviziti.Include(r => r.Clan).FirstOrDefaultAsync(r => r.Id == id);

            if (rekvizit == null)
            {
                return NotFound();//če rekvizit ni najden, vrne 404
            }

            return rekvizit;//vrne najdeni rekvizit
        }

        // POST: api/Rekvizit
        [HttpPost]
        public async Task<ActionResult<Rekvizit>> PostRekvizit(Rekvizit rekvizit)
        {
            //doda nov rekvizit in shrani spremembe
            _context.Rekviziti.Add(rekvizit);
            await _context.SaveChangesAsync();

            //vrne ustvarjeni rekvizit
            return CreatedAtAction(nameof(GetRekvizit), new { id = rekvizit.Id }, rekvizit);
        }

        // PUT: api/Rekvizit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRekvizit(int id, Rekvizit rekvizit)
        {
            if (id != rekvizit.Id)
            {
                return BadRequest();//preveri usklajenost ID-jev, vrne napako če se ne ujemajo
            }

            _context.Entry(rekvizit).State = EntityState.Modified;//označi rekvizit kot spremenjen

            try
            {
                await _context.SaveChangesAsync();//poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RekvizitExists(id))
                {
                    return NotFound();//če rekvizit ne obstaja, vrne 404
                }
                else
                {
                    throw;//če pride do drugih napak pri shranjevanju
                }
            }

            return NoContent();//vrne status 204, ko so spremembe uspešno shranjene
        }

        // DELETE: api/Rekvizit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRekvizit(int id)
        {
            //poišče rekvizit po ID-ju
            var rekvizit = await _context.Rekviziti.FindAsync(id);
            if (rekvizit == null)
            {
                return NotFound();//če rekvizit ni najden, vrne 404
            }

            _context.Rekviziti.Remove(rekvizit);//odstrani rekvizit iz baze
            await _context.SaveChangesAsync();//shrani spremembe

            return NoContent();//vrne status 204 po uspešnem brisanju
        }

        private bool RekvizitExists(int id)
        {
            //preveri, če rekvizit z danim ID-jem obstaja v bazi
            return _context.Rekviziti.Any(e => e.Id == id);
        }
    }
}
