using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Rezervacije
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rezervacija Rezervacija { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rezervacija = await _context.Rezervacije
                .Include(r => r.Termin)
                .Include(r => r.Clan)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Rezervacija == null)
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

            Rezervacija = await _context.Rezervacije.FindAsync(id);

            if (Rezervacija != null)
            {
                _context.Rezervacije.Remove(Rezervacija);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
