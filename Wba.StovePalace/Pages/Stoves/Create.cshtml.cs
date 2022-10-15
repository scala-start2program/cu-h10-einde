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

namespace Wba.StovePalace.Pages.Stoves
{
    public class CreateModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        private readonly IWebHostEnvironment webhostEnvironment;

        public CreateModel(Wba.StovePalace.Data.StoveContext context,
            IWebHostEnvironment webhostEnvironment
)
        {
            _context = context;
            this.webhostEnvironment = webhostEnvironment;
        }

        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(b => b.BrandName).ToList(), "Id", "BrandName");
            ViewData["FuelId"] = new SelectList(_context.Fuel.OrderBy(b => b.FuelName).ToList(), "Id", "FuelName");
            return Page();
        }

        [BindProperty]
        public Stove Stove { get; set; }
        [BindProperty]
        public IFormFile PhotoUpload { get; set; }
        public Availability Availability { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD

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

            _context.Stove.Add(Stove);
            _context.SaveChanges();

            return RedirectToPage("./Index");
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
