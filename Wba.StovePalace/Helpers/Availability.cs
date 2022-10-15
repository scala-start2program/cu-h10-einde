using Wba.StovePalace.Data;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Helpers
{
    public class Availability
    {
        public string UserId { get; } = "";
        public bool IsAdmin { get; } = false;
        public string Email { get; } = "";
        public string ConfigButtonStyle { get; } = "visibility:hidden;";
        public Availability(StoveContext context, HttpContext httpContext)
        {
            string userId = httpContext.Request.Cookies["UserID"];
            if (!string.IsNullOrEmpty(userId))
            {
                userId = Encoding.DecryptString(userId, "P@sw00rd");
                User user = context.User.FirstOrDefault(m => m.Id == int.Parse(userId));
                if (user != null)
                {
                    UserId = userId;
                    IsAdmin = user.IsAdmin;
                    Email = user.Email;
                    if (IsAdmin)
                    {
                        ConfigButtonStyle = "visibility:visible;";
                    }

                }
            }

        }

    }
}
