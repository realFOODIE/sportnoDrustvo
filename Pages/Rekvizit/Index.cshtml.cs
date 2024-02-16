using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

namespace sportnoDrustvo.Pages.Rekviziti
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Rekvizit> Rekvizit { get; set; }

        public async Task OnGetAsync()
        {
            Rekvizit = await _context.Rekviziti
                .Include(r => r.Clan) // Če želite prikazati informacije o članu, ki je izposodil rekvizit
                .ToListAsync();
        }
    }
}
