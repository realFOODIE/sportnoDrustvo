using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sportnoDrustvo.Classes;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Termini
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Termin Termin { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Termini.Add(Termin);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

}
