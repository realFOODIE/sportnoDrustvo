using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class DeleteModel : PageModel //razred za stran za brisanje rekvizita
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public DeleteModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta
        {
            _context = context;
        }

        [BindProperty]
        public Models.Rekvizit Rekvizit { get; set; } //lastnost za shranjevanje rekvizita

        public async Task<IActionResult> OnGetAsync(int? id) //asinhrona metoda za GET zahtevek
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            //poišče rekvizit po id, vključno s povezanim članom
            Rekvizit = await _context.Rekviziti
                .Include(r => r.Clan)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Rekvizit == null) //preveri, če rekvizit obstaja
            {
                return NotFound(); //vrne NotFound, če rekvizit ni najden
            }
            return Page(); //vrne stran za potrditev brisanja
        }

        public async Task<IActionResult> OnPostAsync(int? id) //asinhrona metoda za POST zahtevek
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            Rekvizit = await _context.Rekviziti.FindAsync(id); //poišče rekvizit po id

            if (Rekvizit != null) //preveri, če rekvizit obstaja
            {
                _context.Rekviziti.Remove(Rekvizit); //izbriše rekvizit iz baze
                await _context.SaveChangesAsync(); //shranjuje spremembe
            }

            return RedirectToPage("./Index"); //preusmeri na seznam rekvizitov po uspešnem brisanju
        }
    }
}
