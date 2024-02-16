using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Obvestila
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Obvestilo Obvestilo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Obvestilo == null)
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

            Obvestilo = await _context.Obvestila.FindAsync(id);

            if (Obvestilo != null)
            {
                _context.Obvestila.Remove(Obvestilo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
