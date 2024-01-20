namespace Health_prescription_software_API.Hubs
{
    using Health_prescription_software_API.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IAuthenticationService authenticationService;

        public ChatHub(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task SendMessage(string receiverEgn, string message)
        {
            var senderId = Context.UserIdentifier;
            var receiverId = await authenticationService.GetUserByEgn(receiverEgn);
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            await Clients.User(receiverId!.Id).SendAsync("ReceiveMessage", senderId, message, time);
        }
    }
}


