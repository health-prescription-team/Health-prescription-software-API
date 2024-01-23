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

        public async Task SendMessage(string receiverEgn, string message)
        {
            try
            {
                var senderId = Context.UserIdentifier;
                var receiver = await authenticationService.GetUserByEgn(receiverEgn);
                var time = DateTime.Now;

                await chatService.AddMessage(senderId!, receiver!.Id, senderId!, time, message);

                await Clients.User(receiver!.Id).SendAsync("ReceiveMessage", senderId, message, time.ToString("yyyy-MM-dd HH:mm"));
            }
            catch (Exception)
            {
                // This exception is thrown to the client. Front end job to handle it.
                throw new HubException("Server encountered an unexpected error. Please try again later.");
            }
        }
    }
}


