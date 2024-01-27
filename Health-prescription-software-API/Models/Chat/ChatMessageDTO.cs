namespace Health_prescription_software_API.Models.Chat
{
    public class ChatMessageDTO
    {
        public Guid Id { get; set; }

        public string Message { get; set; } = null!;

        public string MessageTime { get; set; } = null!;

        public string AuthorId { get; set; } = null!;

        public string RecipientId { get; set; } = null!;

        public bool IsRead { get; set; }
    }
}