using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using sportnoDrustvo.Classes; // Namespace za vaše modele
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Trenerji
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Trener> Trener { get; set; }

        public async Task OnGetAsync()
        {
            Trener = await _context.Trenerji.ToListAsync();
        }
    }
}
