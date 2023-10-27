using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string LookingFor { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<Photo> Photos { get; set; } = new();
        public List<UserLike> LikedByUsers { get; set; } = new();
        public List<UserLike> LikedUsers { get; set; } = new();
        public List<Message> MessagesSent { get; set; } = new();
        public List<Message> MessagesReceived { get; set; } = new();
        public List<AppUserRole> UserRoles { get; set; } = new();
    }
}
