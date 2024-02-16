using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele

namespace sportnoDrustvo.Pages.Trenerji
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Trener Trener { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trener = await _context.Trenerji.FirstOrDefaultAsync(m => m.Id == id);

            if (Trener == null)
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

            Trener = await _context.Trenerji.FindAsync(id);

            if (Trener != null)
            {
                _context.Trenerji.Remove(Trener);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
