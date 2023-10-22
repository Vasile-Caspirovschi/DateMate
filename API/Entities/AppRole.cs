using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public List<AppUserRole> UserRoles { get; set; } = new();
    }
}
