using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
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
        public Models.Rekvizit Rekvizit { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rekviziti.Add(Rekvizit);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
