using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
