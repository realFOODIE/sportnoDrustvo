using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele

namespace sportnoDrustvo.Pages.Obvestila
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
            return Page();
        }

        [BindProperty]
        public Models.Obvestilo Obvestilo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Obvestila.Add(Obvestilo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
