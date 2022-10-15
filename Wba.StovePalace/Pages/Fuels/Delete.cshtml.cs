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

namespace Wba.StovePalace.Pages.Fuels
{
    public class DeleteModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public DeleteModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Fuel Fuel { get; set; }
        public Availability Availability { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            if (id == null || _context.Fuel == null)
            {
                return NotFound();
            }

            var fuel = await _context.Fuel.FirstOrDefaultAsync(m => m.Id == id);

            if (fuel == null)
            {
                return NotFound();
            }
            else 
            {
                Fuel = fuel;
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
            if (id == null || _context.Fuel == null)
            {
                return NotFound();
            }
            var fuel = await _context.Fuel.FindAsync(id);

            if (fuel != null)
            {
                Fuel = fuel;
                _context.Fuel.Remove(Fuel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
