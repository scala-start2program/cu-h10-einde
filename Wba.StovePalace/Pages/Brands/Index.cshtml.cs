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

namespace Wba.StovePalace.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Brand> Brands { get;set; }
        public Pagination Pagination { get; private set; }

        public void OnGet(int? pageIndex)
        {
            Pagination = new Pagination(_context.Brand.Count(), pageIndex, 4);

            //Brands = _context.Brand
            //    .Skip(Pagination.FirstObjectIndex)
            //    .Take(4)
            //    .OrderBy(b => b.BrandName).ToList();

            IEnumerable<Brand> query = _context.Brand
                .OrderBy(f => f.BrandName);
            Brands = query
                    .Skip(Pagination.FirstObjectIndex)
                    .Take(4).ToList();
        }
    }
}
