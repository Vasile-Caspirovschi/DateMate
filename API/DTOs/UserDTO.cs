namespace API.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string KnownAs { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
}
