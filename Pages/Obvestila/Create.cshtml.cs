using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; //uvoz potrebnih razredov in modelov

namespace sportnoDrustvo.Pages.Obvestila
{
    public class CreateModel : PageModel //razred strani za ustvarjanje obvestil
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public CreateModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta baze
        {
            _context = context;
        }

        public IActionResult OnGet() //metoda ob obisku strani
        {
            ViewData["TerminId"] = new SelectList(_context.Termini, "Id", "ImeEkipe"); //pripravi seznam terminov za izbirnik
            return Page(); //vrne stran za prikaz
        }

        [BindProperty]
        public Models.Obvestilo Obvestilo { get; set; } //lastnost za shranjevanje obvestila iz obrazca

        public async Task<IActionResult> OnPostAsync() //asinhrona metoda ob oddaji obrazca
        {
            if (!ModelState.IsValid) //preverjanje veljavnosti modela
            {
                return Page(); //vrne stran z obrazcem, če podatki niso veljavni
            }

            _context.Obvestila.Add(Obvestilo); //doda obvestilo v bazo
            await _context.SaveChangesAsync(); //shranjuje spremembe v bazo

            return RedirectToPage("./Index"); //preusmeri na seznam obvestil po uspešnem dodajanju
        }
    }
}
