using Microsoft.AspNetCore.Identity;

namespace Bookshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IdentityUserRole<string> Roles { get; set; }
    }
}
