using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele

namespace sportnoDrustvo.Pages.Trenerji
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
