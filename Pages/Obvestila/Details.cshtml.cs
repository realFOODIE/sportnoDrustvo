using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Obvestila
{
    public class DetailsModel : PageModel //razred za stran podrobnosti obvestila
    {
        private readonly ApplicationDbContext _context; //dostop do konteksta baze

        public DetailsModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta
        {
            _context = context;
        }

        public Obvestilo Obvestilo { get; set; } //lastnost za shranjevanje obvestila

        public async Task<IActionResult> OnGetAsync(int? id) //asinhrona metoda za GET zahtevo
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne, če id ni najden
            }

            //poišče obvestilo po id, vključno s povezanim terminom
            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Obvestilo == null) //preveri, če obvestilo obstaja
            {
                return NotFound(); //vrne, če obvestilo ni najdeno
            }
            return Page(); //prikaže stran s podrobnostmi obvestila
        }
    }
}
