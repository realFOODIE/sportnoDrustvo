using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; //uvoz namespace-a za dostop do modelov in konteksta

namespace sportnoDrustvo.Pages.Obvestila
{
    public class IndexModel : PageModel //razred za prikaz seznama obvestil
    {
        private readonly ApplicationDbContext _context; //instance konteksta za dostop do baze

        public IndexModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta
        {
            _context = context;
        }

        public IList<Models.Obvestilo> Obvestilo { get; set; } //seznam obvestil za prikaz

        public async Task OnGetAsync() //asinhrona metoda za obdelavo zahtevkov GET
        {
            //pridobivanje seznama obvestil iz baze, vključno z informacijami o terminih
            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin) //vključi podatke o povezanih terminih
                .ToListAsync(); //asinhrono pretvori rezultat v seznam
        }
    }
}
