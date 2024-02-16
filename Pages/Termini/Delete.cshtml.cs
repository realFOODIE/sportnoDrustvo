using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using System.Threading.Tasks;
using static sportnoDrustvo.Classes.Models;



namespace sportnoDrustvo.Pages.Termini
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Termin Termin { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Termin = await _context.Termini.FirstOrDefaultAsync(m => m.Id == id);

            if (Termin == null)
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

            Termin = await _context.Termini.FindAsync(id);

            if (Termin != null)
            {
                _context.Termini.Remove(Termin);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
