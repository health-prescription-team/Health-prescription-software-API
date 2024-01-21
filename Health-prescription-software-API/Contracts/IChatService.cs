namespace Health_prescription_software_API.Contracts
{
    using Models.Chat;

    public interface IChatService
    {
        Task AddMessage(string userOneId, string userTwoId, string senderId, DateTime messageTime, string message);

        Task<IEnumerable<ChatMessageDTO>> GetChatMessages(string userOneId, string userTwoId);
    }
}
