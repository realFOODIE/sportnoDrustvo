using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Classes;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Pages.Clani
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Clan Clan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clan = await _context.Clani.FirstOrDefaultAsync(m => m.Id == id); //poišče člana za brisanje

            if (Clan == null)
            {
                return NotFound(); //če član ne obstaja, vrne NotFound
            }
            return Page(); //prikaže stran za potrditev brisanja
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clan = await _context.Clani.FindAsync(id); //poišče člana po ID

            if (Clan != null)
            {
                _context.Clani.Remove(Clan); //odstrani člana iz baze
                await _context.SaveChangesAsync(); //shrani spremembe
            }

            return RedirectToPage("./Index"); //preusmeri nazaj na seznam članov
        }
    }
}
