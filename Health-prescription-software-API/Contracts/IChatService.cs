namespace Health_prescription_software_API.Contracts
{
    using Models.Chat;

    public interface IChatService
    {
        Task AddMessage(string senderId, string recipientId, DateTime messageTime, string message);

        Task<IEnumerable<ChatMessageDTO>> GetChatMessages(string userOneId, string userTwoId);

        Task<ChatUserDetailsDTO> GetUserDetailsByEgn(string egn);

        Task<string> GetUserIdByEgn(string egn);

        Task<bool> UserHasUnreadMessages(string userId);

        Task SetMessageIsRead(string messageId);
    }
}
