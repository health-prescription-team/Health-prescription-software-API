﻿namespace Health_prescription_software_API.Hubs
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Contracts.Validations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using static Common.Roles.RoleConstants;
    using static Common.EntityValidationErrorMessages.Chat;

    [Authorize(Roles = $"{GP}, {Patient}")]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly IChatValidation chatValidation;

        public ChatHub(IChatService chatService,
            IChatValidation chatValidation)
        {
            this.chatService = chatService;
            this.chatValidation = chatValidation;
        }

        public async Task SendMessage(string recipientEgn, string message)
        {
            try
            {
                if (!await chatValidation.IsEngValid(recipientEgn) ||
                    !chatValidation.IsMsgLengthValid(message))
                {
                    var errorMessage = chatValidation.ModelErrors.First();

                    throw new HubException(errorMessage.ErrorMessage);
                }

                var senderId = Context.UserIdentifier;
                var recipientId = await chatService.GetUserIdByEgn(recipientEgn);
                var time = DateTime.Now;

                await chatService.AddMessage(senderId!, recipientId, time, message);

                await Clients.User(recipientId).SendAsync("ReceiveMessage", senderId, message, time.ToString("yyyy-MM-dd HH:mm"));
            }
            catch(HubException ex)
            {
                throw new HubException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw new HubException(ServerErrorMessage);
            }
        }
    }
}


