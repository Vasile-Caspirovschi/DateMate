namespace API.DTOs
{
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}