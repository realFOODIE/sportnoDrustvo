using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele

namespace sportnoDrustvo.Pages.Obvestila
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Obvestilo Obvestilo { get; set; }

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

            ViewData["TerminId"] = new SelectList(_context.Termini, "Id", "ImeEkipe", Obvestilo.TerminId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Obvestilo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Obvestila.Any(e => e.Id == Obvestilo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
