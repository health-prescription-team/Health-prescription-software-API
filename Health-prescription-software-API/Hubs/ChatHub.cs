using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Health_prescription_software_API.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IChatService _chatService;

		public ChatHub(IChatService chatService)
        {
			_chatService = chatService;
		}
        public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");	


		}

	}
}
