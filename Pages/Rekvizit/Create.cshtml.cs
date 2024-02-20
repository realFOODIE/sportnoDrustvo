using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class CreateModel : PageModel //razred za ustvarjanje novega rekvizita
    {
        private readonly ApplicationDbContext _context; //dostop do baze podatkov

        public CreateModel(ApplicationDbContext context) //konstruktor za inicializacijo konteksta baze
        {
            _context = context;
        }

        public IActionResult OnGet() //metoda za GET zahtevek
        {
            return Page(); //vrne stran za ustvarjanje rekvizita
        }

        [BindProperty]
        public Models.Rekvizit Rekvizit { get; set; } //lastnost za vezavo podatkov iz obrazca

        public async Task<IActionResult> OnPostAsync() //asinhrona metoda za obdelavo POST zahtevka
        {
            if (!ModelState.IsValid) //preverjanje veljavnosti modela
            {
                return Page(); //če model ni veljaven, vrne stran z obstoječimi podatki
            }

            _context.Rekviziti.Add(Rekvizit); //dodajanje novega rekvizita v bazo
            await _context.SaveChangesAsync(); //shranjevanje sprememb v bazi

            return RedirectToPage("./Index"); //preusmeritev na seznam rekvizitov po uspešnem dodajanju
        }
    }
}
