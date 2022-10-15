using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wba.StovePalace.Data;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Fuels
{
    public class CreateModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public CreateModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }
        public Availability Availability { get; set; }
        [BindProperty]
        public Models.Fuel Fuel { get; set; }
        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            return Page();
        }        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Fuel.Add(Fuel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
