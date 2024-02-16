using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Rekvizit Rekvizit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rekvizit = await _context.Rekviziti
                .Include(r => r.Clan)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Rekvizit == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rekvizit = await _context.Rekviziti.FindAsync(id);

            if (Rekvizit != null)
            {
                _context.Rekviziti.Remove(Rekvizit);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
