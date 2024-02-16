using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;


namespace sportnoDrustvo.Pages.Clani
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
        public Clan Clan { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clani.Add(Clan);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
