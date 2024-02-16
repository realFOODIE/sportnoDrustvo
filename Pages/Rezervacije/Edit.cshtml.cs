using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Rezervacije
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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

            ViewData["TerminId"] = new SelectList(_context.Termini, "Id", "ImeEkipe");
            ViewData["ClanId"] = new SelectList(_context.Clani, "Id", "Ime");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Rezervacija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rezervacije.Any(e => e.Id == Rezervacija.Id))
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
