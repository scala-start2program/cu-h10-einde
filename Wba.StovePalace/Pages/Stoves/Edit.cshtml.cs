using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Wba.StovePalace.Helpers;

namespace Wba.StovePalace.Pages.Stoves
{
    public class EditModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        private readonly IWebHostEnvironment webhostEnvironment;

        public EditModel(Wba.StovePalace.Data.StoveContext context,
            IWebHostEnvironment webhostEnvironment)
        {
            _context = context;
            this.webhostEnvironment = webhostEnvironment;
        }

        [BindProperty]
        public Stove Stove { get; set; } = default!;

        [BindProperty]
        public IFormFile PhotoUpload { get; set; }
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

            var stove =  await _context.Stove.FirstOrDefaultAsync(m => m.Id == id);
            if (stove == null)
            {
                return NotFound();
            }
            Stove = stove;
           ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(b => b.BrandName).ToList(), "Id", "BrandName");
           ViewData["FuelId"] = new SelectList(_context.Fuel.OrderBy(f => f.FuelName).ToList(), "Id", "FuelName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
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
            if (PhotoUpload != null)
            {
                if (Stove.ImagePath != null)
                {
                    string filePath = Path.Combine(webhostEnvironment.WebRootPath, "images", Stove.ImagePath);
                    System.IO.File.Delete(filePath);
                }
                Stove.ImagePath = ProcessUploadedFile();
            }

            _context.Add(Stove).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoveExists(Stove.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool StoveExists(int id)
        {
          return _context.Stove.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (PhotoUpload != null)
            {
                string uploadFolder = Path.Combine(webhostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + PhotoUpload.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    PhotoUpload.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
