namespace Health_prescription_software_API.Services
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Data;
    using Health_prescription_software_API.Data.Entities.Chat;
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
            var conversation = await dbContext.Conversations.FindAsync(userOneId, userTwoId) ?? 
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

        public Task<IEnumerable<ChatMessage>> GetChatMessages(string userOneId, string userTwoId)
        {
            throw new NotImplementedException();
        }

        public Task<Conversation?> GetConversation(string userOneId, string userTwoId)
        {
            throw new NotImplementedException();
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
