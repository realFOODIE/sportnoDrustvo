using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using sportnoDrustvo.Classes; 
using static sportnoDrustvo.Classes.Models; 

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")] //določa osnovno pot za API klice
    [ApiController] //označuje, da je ta razred controller in omogoča nekatere funkcionalnosti, kot so model binding, itd.
    public class ClanController : ControllerBase //razširi ControllerBase za gradnjo API-ja
    {
        private readonly ApplicationDbContext _context; //instanca DbContext-a za dostop do baze

        public ClanController(ApplicationDbContext context) //konstruktor, ki sprejme DbContext
        {
            _context = context;
        }

        // GET: api/Clan
        [HttpGet] //določa, da je to HTTP GET metoda
        public async Task<ActionResult<IEnumerable<Clan>>> GetClani() //asinhrono vrne vse člane
        {
            return await _context.Clani.Include(c => c.Obvestila).ToListAsync(); //vključuje obvestila povezana s člani
        }

        // GET: api/Clan/5
        [HttpGet("{id}")] //določa, da je to HTTP GET metoda, ki sprejme parameter id
        public async Task<ActionResult<Clan>> GetClan(int id) //asinhrono vrne člana z določenim id-jem
        {
            var clan = await _context.Clani.Include(c => c.Obvestila).FirstOrDefaultAsync(c => c.Id == id); //poišče člana po id-ju

            if (clan == null) //preveri, če član obstaja
            {
                return NotFound(); //vrne 404, če član ni najden
            }

            return clan; //vrne člana
        }

        // POST: api/Clan
        [HttpPost] //določa, da je to HTTP POST metoda
        public async Task<ActionResult<Clan>> PostClan(Clan clan) //asinhrono doda novega člana
        {
            _context.Clani.Add(clan); //doda člana v DbContext
            await _context.SaveChangesAsync(); //shrani spremembe v bazo

            return CreatedAtAction("GetClan", new { id = clan.Id }, clan); //vrne ustvarjenega člana
        }

        // PUT: api/Clan/5
        [HttpPut("{id}")] //določa, da je to HTTP PUT metoda, ki sprejme parameter id
        public async Task<IActionResult> PutClan(int id, Clan clan) //asinhrono posodobi člana
        {
            if (id != clan.Id) //preveri, če id-ji se ujemajo
            {
                return BadRequest(); //vrne 400, če id-ji ne ustrezajo
            }

            _context.Entry(clan).State = EntityState.Modified; //označi stanje objekta kot modificirano

            try
            {
                await _context.SaveChangesAsync(); //poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException) //ulovi izjemo, če pride do konflikta pri posodabljanju
            {
                if (!ClanExists(id)) //preveri, če član obstaja
                {
                    return NotFound(); //vrne 404, če član ne obstaja
                }
                else
                {
                    throw; 
                }
            }

            return NoContent(); //vrne 204, ko je posodobitev uspešna
        }

        // DELETE: api/Clan/5
        [HttpDelete("{id}")] //določa, da je to HTTP DELETE metoda, ki sprejme parameter id
        public async Task<IActionResult> DeleteClan(int id) //asinhrono izbriše člana
        {
            var clan = await _context.Clani.FindAsync(id); //poišče člana po id-ju
            if (clan == null) //preveri, če član obstaja
            {
                return NotFound(); //vrne 404, če član ni najden
            }

            _context.Clani.Remove(clan); //odstrani člana iz DbContext-a
            await _context.SaveChangesAsync(); //shrani spremembe v bazo

            return NoContent(); //vrne 204, ko je brisanje uspešno
        }

        private bool ClanExists(int id) //preveri, če član z določenim id-jem obstaja
        {
            return _context.Clani.Any(e => e.Id == id); //vrne true, če član obstaja
        }
    }
}
