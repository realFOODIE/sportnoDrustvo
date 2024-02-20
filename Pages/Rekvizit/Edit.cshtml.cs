using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class EditModel : PageModel //razred za urejanje rekvizitov
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public EditModel(ApplicationDbContext context) //konstruktor za inicializacijo _context
        {
            _context = context;
        }

        [BindProperty]
        public Models.Rekvizit Rekvizit { get; set; } //lastnost za vezavo na rekvizit

        public async Task<IActionResult> OnGetAsync(int? id) //metoda za obdelavo GET zahtevka
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            //poišče rekvizit po id
            Rekvizit = await _context.Rekviziti.FirstOrDefaultAsync(m => m.Id == id);

            if (Rekvizit == null) //preveri, če rekvizit obstaja
            {
                return NotFound(); //vrne NotFound, če rekvizit ni najden
            }
            return Page(); //vrne stran za urejanje
        }

        public async Task<IActionResult> OnPostAsync() //metoda za obdelavo POST zahtevka
        {
            if (!ModelState.IsValid) //preveri veljavnost modela
            {
                return Page(); //vrne stran, če model ni veljaven
            }

            _context.Attach(Rekvizit).State = EntityState.Modified; //označi rekvizit kot spremenjen

            try
            {
                await _context.SaveChangesAsync(); //poskuša shraniti spremembe
            }
            catch (DbUpdateConcurrencyException) //ulovi izjemo pri konfliktu verzij
            {
                if (!_context.Rekviziti.Any(e => e.Id == Rekvizit.Id)) //preveri, če rekvizit še obstaja
                {
                    return NotFound(); //vrne NotFound, če rekvizit ne obstaja
                }
                else
                {
                    throw; //sproži izjemo, če je drug vzrok za konflikt
                }
            }

            return RedirectToPage("./Index"); //preusmeri na seznam rekvizitov po uspešnem urejanju
        }
    }
}
