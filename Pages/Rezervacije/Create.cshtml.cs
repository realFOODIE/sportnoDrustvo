using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Rezervacije
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["TerminId"] = new SelectList(_context.Termini, "Id", "ImeEkipe");
            ViewData["ClanId"] = new SelectList(_context.Clani, "Id", "Ime");
            return Page();
        }

        [BindProperty]
        public Rezervacija Rezervacija { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rezervacije.Add(Rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
