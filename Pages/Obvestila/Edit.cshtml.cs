using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; //uvoz modelov in konteksta baze

namespace sportnoDrustvo.Pages.Obvestila
{
    public class EditModel : PageModel //razred za urejanje obvestil
    {
        private readonly ApplicationDbContext _context; //kontekst za dostop do baze

        public EditModel(ApplicationDbContext context) //konstruktor za inicializacijo _context
        {
            _context = context;
        }

        [BindProperty]
        public Models.Obvestilo Obvestilo { get; set; } //lastnost za shranjevanje obvestila

        public async Task<IActionResult> OnGetAsync(int? id) //pripravi podatke za GET zahtevo
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            //poišče obvestilo z id in vključi povezani termin
            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Obvestilo == null) //preveri, če obvestilo obstaja
            {
                return NotFound(); //vrne NotFound, če obvestilo ni najdeno
            }

            //pripravi seznam terminov za izbirnik
            ViewData["TerminId"] = new SelectList(_context.Termini, "Id", "ImeEkipe", Obvestilo.TerminId);
            return Page(); //vrne stran za urejanje
        }

        public async Task<IActionResult> OnPostAsync() //obdela POST zahtevo
        {
            if (!ModelState.IsValid) //preveri veljavnost modela
            {
                return Page(); //vrne stran z napakami, če model ni veljaven
            }

            _context.Attach(Obvestilo).State = EntityState.Modified; //označi obvestilo kot spremenjeno

            try
            {
                await _context.SaveChangesAsync(); //poskusi shraniti spremembe
            }
            catch (DbUpdateConcurrencyException) //ulovi izjemo, če pride do konflikta
            {
                if (!_context.Obvestila.Any(e => e.Id == Obvestilo.Id)) //preveri, če obvestilo še obstaja
                {
                    return NotFound(); //vrne NotFound, če obvestilo ne obstaja
                }
                else
                {
                    throw; //sproži izjemo naprej, če obstaja drug vzrok za izjemo
                }
            }

            return RedirectToPage("./Index"); //preusmeri na seznam obvestil po uspešnem urejanju
        }
    }
}
