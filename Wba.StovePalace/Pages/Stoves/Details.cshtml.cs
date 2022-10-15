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
    public class DetailsModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public DetailsModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public Stove Stove { get; set; }
        public int? PreviousId { get; set; }
        public int? NextId { get; set; }
        private IList<Stove> Stoves { get; set; }
        public Availability Availability { get; private set; }

        public IActionResult OnGet(int? id)
        {
            Availability = new Availability(_context, HttpContext); 
            if (id == null || _context.Stove == null)
            {
                return NotFound();
            }

            var stove =  _context.Stove
                .Include(b => b.Brand)
                .Include(f => f.Fuel).ToList()
                .FirstOrDefault(m => m.Id == id);

            Stoves = _context.Stove
                .Include(b => b.Brand)
                .Include(f => f.Fuel).ToList();
            Stoves = Stoves.OrderBy(s => s.Brand.BrandName)
                .ThenBy(s => s.Fuel.FuelName).ToList();
            PreviousId = null;
            NextId = null;
            for (int i = 0; i < Stoves.Count; i++)
            {
                if (((Stove)Stoves[i]).Id == id)
                {
                    if (i > 0)
                        PreviousId = ((Stove)Stoves[i - 1]).Id;
                    if (i < Stoves.Count - 1)
                        NextId = ((Stove)Stoves[i + 1]).Id;
                    break;
                }
            }
            if (PreviousId == null) PreviousId = id;
            if (NextId == null) NextId = id;


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
    }
}
