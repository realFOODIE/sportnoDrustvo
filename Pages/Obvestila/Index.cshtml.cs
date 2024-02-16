using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele

namespace sportnoDrustvo.Pages.Obvestila
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Obvestilo> Obvestilo { get; set; }

        public async Task OnGetAsync()
        {
            Obvestilo = await _context.Obvestila
                .Include(o => o.Termin) // Preverite pravilnost navigacijskih lastnosti
                .ToListAsync();
        }
    }
}
