using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using sportnoDrustvo.Controllers;
using static sportnoDrustvo.Classes.Models;


namespace sportnoDrustvo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Clan> Clani { get; set; }

        public async Task OnGetAsync()
        {
            Clani = await _context.Clani.ToListAsync();
        }
    }
}
