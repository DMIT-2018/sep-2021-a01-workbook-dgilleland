using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int? StaffId { get; set; }
    }
}
