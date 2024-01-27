namespace Health_prescription_software_API.Models.Chat
{
    public class ChatUserDetailsDTO
    {
        public string UserId { get; set; } = null!;

        public byte[] UserImage { get; set; } = null!;

        public string FullName { get; set; } = null!;
    }
}