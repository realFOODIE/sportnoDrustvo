using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Clani
{
    public class EditModel : PageModel //deduje od PageModel za Razor Pages
    {
        private readonly ApplicationDbContext _context; //kontekst za dostop do baze

        public EditModel(ApplicationDbContext context) //konstruktor za inicializacijo _context
        {
            _context = context;
        }

        [BindProperty]
        public Clan Clan { get; set; } //lastnost za povezovanje s podatki iz obrazca

        public async Task<IActionResult> OnGetAsync(int? id) //asinhrona metoda za GET zahtevo
        {
            if (id == null)
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            Clan = await _context.Clani.FirstOrDefaultAsync(m => m.Id == id); //poišče člana po id

            if (Clan == null)
            {
                return NotFound(); //vrne NotFound, če član ni najden
            }
            return Page(); //vrne stran za urejanje
        }

        public async Task<IActionResult> OnPostAsync() //asinhrona metoda za POST zahtevo
        {
            if (!ModelState.IsValid) //preveri veljavnost modela
            {
                return Page(); //vrne stran z napakami validacije, če model ni veljaven
            }

            _context.Attach(Clan).State = EntityState.Modified; //označi stanje člana kot spremenjeno

            try
            {
                await _context.SaveChangesAsync(); //poskusi shraniti spremembe v bazo
            }
            catch (DbUpdateConcurrencyException) //ulovi izjemo, če pride do konflikta pri posodobitvi
            {
                if (!_context.Clani.Any(e => e.Id == Clan.Id)) //preveri, če član še obstaja
                {
                    return NotFound(); //vrne NotFound, če član ne obstaja
                }
                else
                {
                    throw; //sproži izjemo naprej, če obstaja drug vzrok za izjemo
                }
            }

            return RedirectToPage("./Index"); //preusmeri na seznam članov po uspešnem urejanju
        }
    }
}
