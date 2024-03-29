﻿namespace Health_prescription_software_API.Services
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
        private readonly HealthPrescriptionDbContext _context;

        public ChatService(HealthPrescriptionDbContext dbContext)
        {
            this._context = dbContext;
        }

        public async Task AddMessage(string senderId, string recipientId, DateTime messageTime, string message)
        {
            ChatMessage chatMessage = new()
            {
                AuthorId = senderId,
                RecipientId = recipientId,
                MessageTime = messageTime,
                Message = message
            };

            await _context.Messages.AddAsync(chatMessage);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetChatMessages(string userOneId, string userTwoId)
        {
            var conversation = _context.Messages
                .Where(m => m.AuthorId == userOneId && m.RecipientId == userTwoId ||
                       m.AuthorId == userTwoId && m.RecipientId == userOneId);

            await conversation.Where(m => m.AuthorId != userOneId)
                .ExecuteUpdateAsync(m => m.SetProperty(p => p.IsRead, p => true));

            var messages = await conversation
                .OrderByDescending(m => m.MessageTime)
                .Select(m => new ChatMessageDTO
                {
                    Id = m.Id,
                    Message = m.Message,
                    MessageTime = m.MessageTime.ToString("yyyy-MM-dd HH:mm"),
                    AuthorId = m.AuthorId,
                    RecipientId = m.RecipientId,
                    IsRead = m.IsRead
                }).ToArrayAsync();

            return messages ?? [];
        }

        public async Task<string> GetUserIdByEgn(string egn)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Egn == egn);

            return user!.Id;
        }

        public async Task<bool> UserHasUnreadMessages(string userId)
        {
            var messages = _context.Messages
                .Where(m => m.RecipientId == userId);

            return await messages.AnyAsync(m => m.IsRead == false);
        }

        public async Task SetMessageIsRead(string messageId)
        {
            var message = await _context.Messages.FindAsync(Guid.Parse(messageId));

            message!.IsRead = true;

            await _context.SaveChangesAsync();
        }

        public async Task<ChatUserDetailsDTO> GetUserDetailsByEgn(string egn)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Egn == egn);

            return new ChatUserDetailsDTO
            {
                UserId = user!.Id,
                FullName = $"{user.FirstName} {(string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName + " ")}{user.LastName}",
                UserImage = user.ProfilePicture!
            };
        }
    }
}
