using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Trenerji
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
            return Page();
        }

        [BindProperty]
        public Models.Trener Trener { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Trenerji.Add(Trener);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
