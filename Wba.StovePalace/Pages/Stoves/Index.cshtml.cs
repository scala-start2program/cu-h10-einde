using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Stoves
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        private readonly int ItemsPerPage = 3;
        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Stove> Stoves { get;set; } = default!;
        public List<SelectListItem> AvailableBrands { get; set; }
        public List<SelectListItem> AvailableFuels { get; set; }
        public int? SelectedBrandId { get; set; }
        public int? SelectedFuelId { get; set; }
        public Pagination Pagination { get; private set; }
        public Availability Availability { get; set; }
        public void OnGet(int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext); 
            PopulateCollections(null, null, pageIndex);
        }
        public void OnPost(int? brandFilter, int? fuelFilter, int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext); 
            SelectedBrandId = brandFilter;
            SelectedFuelId = fuelFilter;
            PopulateCollections(brandFilter, fuelFilter, pageIndex);
        }

        private void PopulateCollections(int? brandFilter, int? fuelFilter, int? pageIndex)
        {
            // int stoveCount = _context.Stove.Count();
            int stoveCount = 0;
            if (brandFilter == null && fuelFilter == null)
                stoveCount = _context.Stove.Count();
            else if (brandFilter != null && fuelFilter == null)
                stoveCount = _context.Stove.Count(b => b.BrandId.Equals(brandFilter));
            else if (brandFilter == null && fuelFilter != null)
                stoveCount = _context.Stove.Count(f => f.FuelId.Equals(fuelFilter));
            else
                stoveCount = _context.Stove.Count(s => s.BrandId.Equals(brandFilter) && s.FuelId.Equals(fuelFilter));

            Pagination = new Pagination(stoveCount, pageIndex, ItemsPerPage);

            IQueryable<Stove> query;
            if (brandFilter == null && fuelFilter == null)
            {
                query = _context.Stove
                    .Include(b => b.Brand)
                    .Include(f => f.Fuel)
                    .OrderBy(b => b.Brand.BrandName)
                    .ThenBy(f => f.Fuel.FuelName);
            }
            else if (brandFilter != null && fuelFilter == null)
            {
                query = _context.Stove
                    .Where(b => b.BrandId.Equals(brandFilter))
                    .Include(b => b.Brand)
                    .Include(f => f.Fuel)
                    .OrderBy(b => b.Brand.BrandName)
                    .ThenBy(f => f.Fuel.FuelName);
            }
            else if (brandFilter == null && fuelFilter != null)
            {
                query = _context.Stove
                    .Where(b => b.FuelId.Equals(fuelFilter))
                    .Include(b => b.Brand)
                    .Include(f => f.Fuel)
                    .OrderBy(b => b.Brand.BrandName)
                    .ThenBy(f => f.Fuel.FuelName);
            }
            else
            {
                query = _context.Stove
                    .Where(b => b.FuelId.Equals(fuelFilter) && b.FuelId.Equals(fuelFilter))
                    .Include(b => b.Brand)
                    .Include(f => f.Fuel)
                    .OrderBy(b => b.Brand.BrandName)
                    .ThenBy(f => f.Fuel.FuelName);
            }
            Stoves = query
                .Skip(Pagination.FirstObjectIndex)
                .Take(ItemsPerPage)
                .ToList();

            AvailableBrands = _context.Brand.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.BrandName
                }).ToList();
            AvailableBrands = AvailableBrands.OrderBy(b => b.Text).ToList();
            AvailableFuels = _context.Fuel.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FuelName
                }).ToList();
            AvailableFuels = AvailableFuels.OrderBy(b => b.Text).ToList();

            AvailableBrands.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle merken ---"
            });
            AvailableFuels.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle brandstoffen ---"
            });
        }

    }
}
