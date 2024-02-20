using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Clani
{
    public class IndexModel : PageModel //razširi PageModel za uporabo na Razor Pages
    {
        private readonly ApplicationDbContext _context; //kontekst za dostop do baze

        public IndexModel(ApplicationDbContext context) //konstruktor za inicializacijo _context
        {
            _context = context;
        }

        public IList<Clan> Clani { get; set; } //seznami članov za prikaz

        public async Task OnGetAsync() //asinhrona metoda za obdelavo GET zahtev
        {
            Clani = await _context.Clani.ToListAsync(); //pridobi vse člane iz baze
        }
    }
}
