using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities;

namespace Health_prescription_software_API.Services
{
	public class ChatService : IChatService
	{
		private readonly HealthPrescriptionDbContext context;

		public ChatService(HealthPrescriptionDbContext context)
        {
			this.context = context;
		}

		public List<ChatMessage> GetChatHistory(string senderId, string receiverId)
		{
			var messages = context.ChatMessages.Where(m => m.SenderId == senderId && m.ReceiverId == receiverId).OrderBy(m=>m.Timestamp).ToList();
			return messages;
		}

		public async void StoreMessage(ChatMessage message)
		{
			await context.ChatMessages.AddAsync(message);
			await context.SaveChangesAsync();
			
		}
	}
}
