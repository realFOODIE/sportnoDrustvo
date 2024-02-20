using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class DetailsModel : PageModel //razred za prikaz podrobnosti o rekvizitu
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public DetailsModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta
        {
            _context = context;
        }

        public Models.Rekvizit Rekvizit { get; set; } //lastnost za shranjevanje rekvizita

        public async Task<IActionResult> OnGetAsync(int? id) //asinhrona metoda za obdelavo GET zahtevka
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
            return Page(); //vrne stran s podrobnostmi o rekvizitu
        }
    }
}
