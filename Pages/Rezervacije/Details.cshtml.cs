using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Rezervacije
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
