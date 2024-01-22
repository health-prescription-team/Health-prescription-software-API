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
                .OrderByDescending(m => m.MessageTime)
                .Select(m => new ChatMessageDTO
                {
                    Id = m.Id,
                    Message = m.Message,
                    MessageTime = m.MessageTime.ToString("yyyy-MM-dd HH:mm"),
                    AuthorId = m.AuthorId,
                    IsRead = m.IsRead
                }).ToArray();

            return messages ?? [];
        }

        public async Task<string?> GetUserIdByEgn(string egn)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Egn == egn);

            return user?.Id;
        }

        public async Task<bool> UserHasUnreadMessages(string userId)
        {
            var conversations = dbContext.Conversations
                .Include(c => c.Messages)
                .Where(c => c.UserOneId == userId || c.UserTwoId == userId);

            return await conversations.AnyAsync(c => c.Messages.Any(m => m.IsRead == false));
        }

        public async Task SetMessageIsRead(string messageId)
        {
            var message = await dbContext.Messages.FindAsync(Guid.Parse(messageId));

            message!.IsRead = true;

            await dbContext.SaveChangesAsync();
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
