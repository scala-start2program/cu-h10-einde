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
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        public Availability Availability { get; set; }
        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Models.Fuel> Fuels { get;set; } = default!;
        public Pagination Pagination { get; private set; }
        public void OnGet(int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                Response.Redirect("../Stoves/Index");
            }
            Pagination = new Pagination(_context.Fuel.Count(), pageIndex, 4);

            IQueryable<Fuel> query = _context.Fuel
                .OrderBy(f => f.FuelName);
            Fuels = query
                .Skip(Pagination.FirstObjectIndex)
                .Take(4).ToList();
        }

    }
}
