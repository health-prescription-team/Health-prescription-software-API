namespace Health_prescription_software_API.Hubs
{
    using Health_prescription_software_API.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using static Common.Roles.RoleConstants;

    [Authorize(Roles = $"{GP}, {Patient}")]
    public class ChatHub : Hub
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IChatService chatService;

        public ChatHub(IAuthenticationService authenticationService,
            IChatService chatService)
        {
            this.authenticationService = authenticationService;
            this.chatService = chatService;
        }

        public async Task SendMessage(string recipientEgn, string message)
        {
            try
            {
                var senderId = Context.UserIdentifier;
                var recipient = await authenticationService.GetUserByEgn(recipientEgn);
                var time = DateTime.Now;

                await chatService.AddMessage(senderId!, recipient!.Id, time, message);

                await Clients.User(recipient!.Id).SendAsync("ReceiveMessage", senderId, message, time.ToString("yyyy-MM-dd HH:mm"));
            }
            catch (Exception)
            {
                // This exception is thrown to the client. Front end job to handle it.
                throw new HubException("Server encountered an unexpected error. Please try again later.");
            }
        }
    }
}


