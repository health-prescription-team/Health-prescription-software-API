namespace Health_prescription_software_API.Services
{
    using Contracts;
    using Data;
    using Data.Entities.Chat;
    using Models.Chat;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ChatService : IChatService
    {
        private readonly HealthPrescriptionDbContext dbContext;

        public ChatService(HealthPrescriptionDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddMessage(string userOneId, string userTwoId, string senderId, DateTime messageTime, string message)
        {
            var conversation = await this.GetConversation(userOneId, userTwoId) ??
                               await this.CreateConversation(userOneId, userTwoId);

            ChatMessage chatMessage = new()
            {
                AuthorId = senderId,
                MessageTime = messageTime,
                Message = message
            };

            conversation.Messages.Add(chatMessage);

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetChatMessages(string userOneId, string userTwoId)
        {
            var conversation = await this.GetConversation(userOneId, userTwoId);

            var messages = conversation?.Messages
                .Select(m => new ChatMessageDTO
                {
                    Message = m.Message,
                    MessageTime = m.MessageTime.ToString("yyyy-MM-dd HH:mm"),
                    AuthorId = m.AuthorId,
                    IsRead = m.IsRead
                }).ToArray();

            return messages ?? [];
        }

        private async Task<Conversation?> GetConversation(string userOneId, string userTwoId)
        {
            return await dbContext.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.UserOneId == userOneId && c.UserTwoId == userTwoId ||
                                          c.UserOneId == userTwoId && c.UserTwoId == userOneId);
        }

        private async Task<Conversation> CreateConversation(string userOneId, string userTwoId)
        {
            Conversation conversation = new()
            {
                UserOneId = userOneId,
                UserTwoId = userTwoId
            };

            await dbContext.Conversations.AddAsync(conversation);
            await dbContext.SaveChangesAsync();

            return conversation;
        }
    }
}
