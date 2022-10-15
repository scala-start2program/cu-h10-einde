using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public RegisterModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Password = Wba.StovePalace.Helpers.Encoding.EncodePassword(User.Password);
            _context.User.Add(User);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Page();
            }
            string IdCookie = Wba.StovePalace.Helpers.Encoding.EncryptString(User.Id.ToString(), "P@sw00rd");
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(365));
            HttpContext.Response.Cookies.Append("UserId", IdCookie, cookieOptions);
            return RedirectToPage("../Stoves/Index");

        }

    }
}
