using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
