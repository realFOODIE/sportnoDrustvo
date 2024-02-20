using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Obvestila
{
    public class DeleteModel : PageModel //razred za stran brisanja obvestila
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public DeleteModel(ApplicationDbContext context) //konstruktor za inicializacijo baze
        {
            _context = context;
        }

        [BindProperty]
        public Obvestilo Obvestilo { get; set; } //lastnost za shranjevanje obvestila, ki bo izbrisano

        public async Task<IActionResult> OnGetAsync(int? id) //obdelava GET zahteve
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne, če id ni najden
            }

            //poišče obvestilo z id, vključno s terminom
            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Obvestilo == null) //preveri, če obvestilo obstaja
            {
                return NotFound(); //vrne, če obvestilo ni najdeno
            }
            return Page(); //prikaže stran za potrditev brisanja
        }

        public async Task<IActionResult> OnPostAsync(int? id) //obdelava POST zahteve za brisanje
        {
            if (id == null) //preveri, če id obstaja
            {
                return NotFound(); //vrne, če id ni najden
            }

            //najde obvestilo za brisanje
            Obvestilo = await _context.Obvestila.FindAsync(id);

            if (Obvestilo != null) //preveri, če obvestilo obstaja
            {
                _context.Obvestila.Remove(Obvestilo); //izbriše obvestilo iz baze
                await _context.SaveChangesAsync(); //shranjuje spremembe
            }

            return RedirectToPage("./Index"); //preusmeri na seznam obvestil po uspešnem brisanju
        }
    }
}
