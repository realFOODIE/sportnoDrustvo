using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using sportnoDrustvo.Classes;

using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Rezervacije
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Rezervacija> Rezervacija { get; set; }

        public async Task OnGetAsync()
        {
            Rezervacija = await _context.Rezervacije
                .Include(r => r.Termin)
                .Include(r => r.Clan)
                .ToListAsync();
        }
    }
}
