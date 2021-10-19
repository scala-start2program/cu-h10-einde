using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Models;
using Wba.StovePalace.Helpers;

namespace Wba.StovePalace.Pages.Fuels
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Fuel> Fuels { get;set; }
        public Pagination Pagination { get; private set; }

        public void OnGet(int? pageIndex)
        {
            Pagination = new Pagination(_context.Fuel.Count(), pageIndex, 4);

            //Fuels = _context.Fuel
            //    .Skip(Pagination.FirstObjectIndex)
            //    .Take(4)
            //    .OrderBy(f => f.FuelName).ToList();

            IEnumerable<Fuel> query = _context.Fuel
                .OrderBy(f => f.FuelName);
            Fuels = query
                    .Skip(Pagination.FirstObjectIndex)
                    .Take(4).ToList();

        }
    }
}
