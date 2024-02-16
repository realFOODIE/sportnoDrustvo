using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clan>>> GetClani()
        {
            return await _context.Clani.Include(c => c.Obvestila).ToListAsync();
        }

        // GET: api/Clan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clan>> GetClan(int id)
        {
            var clan = await _context.Clani.Include(c => c.Obvestila).FirstOrDefaultAsync(c => c.Id == id);

            if (clan == null)
            {
                return NotFound();
            }

            return clan;
        }

        // POST: api/Clan
        [HttpPost]
        public async Task<ActionResult<Clan>> PostClan(Clan clan)
        {
            _context.Clani.Add(clan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClan", new { id = clan.Id }, clan);
        }

        // PUT: api/Clan/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClan(int id, Clan clan)
        {
            if (id != clan.Id)
            {
                return BadRequest();
            }

            _context.Entry(clan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClanExists(id))
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

        // DELETE: api/Clan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClan(int id)
        {
            var clan = await _context.Clani.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }

            _context.Clani.Remove(clan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClanExists(int id)
        {
            return _context.Clani.Any(e => e.Id == id);
        }
    }
}
