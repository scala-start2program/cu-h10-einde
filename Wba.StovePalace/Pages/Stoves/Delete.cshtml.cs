using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Stoves
{
    public class DeleteModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public DeleteModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Stove Stove { get; set; }
        public Availability Availability { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            if (id == null || _context.Stove == null)
            {
                return NotFound();
            }

            var stove = await _context.Stove
                .Include(b => b.Brand)
                .Include(f => f.Fuel)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stove == null)
            {
                return NotFound();
            }
            else 
            {
                Stove = stove;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            if (id == null || _context.Stove == null)
            {
                return NotFound();
            }
            var stove = await _context.Stove.FindAsync(id);

            if (stove != null)
            {
                Stove = stove;
                _context.Stove.Remove(Stove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
