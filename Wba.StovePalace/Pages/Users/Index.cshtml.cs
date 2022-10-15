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

namespace Wba.StovePalace.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        public Availability Availability { get; set; }
        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                Response.Redirect("../Stoves/Index");
            }
            if (_context.User != null)
            {
                User = await _context.User.ToListAsync();
            }
        }
    }
}
