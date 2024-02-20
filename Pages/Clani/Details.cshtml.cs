using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Clani
{
    public class DetailsModel : PageModel //razširi PageModel za uporabo na Razor Pages
    {
        private readonly ApplicationDbContext _context; //kontekst za dostop do baze

        public DetailsModel(ApplicationDbContext context) //konstruktor za inicializacijo _context
        {
            _context = context;
        }

        public Clan Clan { get; set; } //lastnost Clan za shranjevanje podatkov člana

        public async Task<IActionResult> OnGetAsync(int? id) //asinhrona metoda za obdelavo GET zahteve z ID-jem člana
        {
            if (id == null) //preveri, če id ni podan
            {
                return NotFound(); //vrne NotFound, če id ni podan
            }

            Clan = await _context.Clani.FirstOrDefaultAsync(m => m.Id == id); //poišče člana po id

            if (Clan == null) //preveri, če član ni najden
            {
                return NotFound(); //vrne NotFound, če član ne obstaja
            }
            return Page(); //vrne stran z detajli člana, če je najden
        }
    }
}
