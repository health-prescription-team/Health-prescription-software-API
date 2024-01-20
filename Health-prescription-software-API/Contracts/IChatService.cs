namespace Health_prescription_software_API.Contracts
{
    using Health_prescription_software_API.Data.Entities.Chat;

    public interface IChatService
    {
        Task AddMessage(string userOneId, string userTwoId, string senderId, DateTime messageTime, string message);

        Task<Conversation?> GetConversation(string userOneId, string userTwoId);

        Task<IEnumerable<ChatMessage>> GetChatMessages(string userOneId, string userTwoId);
    }
}
