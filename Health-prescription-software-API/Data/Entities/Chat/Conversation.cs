namespace Health_prescription_software_API.Data.Entities.Chat
{
    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(UserOneId), nameof(UserTwoId))]
    public class Conversation
    {
        public Conversation()
        {
            this.Messages = new HashSet<ChatMessage>();
        }

        public string UserOneId { get; set; } = null!;

        public string UserTwoId { get; set; } = null!;

        public virtual ICollection<ChatMessage> Messages { get; set; }
    }
}