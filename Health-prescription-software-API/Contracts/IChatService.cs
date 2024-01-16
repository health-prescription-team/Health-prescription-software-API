using Health_prescription_software_API.Data.Entities;

namespace Health_prescription_software_API.Contracts
{
	public interface IChatService
	{
		List<ChatMessage> GetChatHistory(string senderId, string receiverId);

		void StoreMessage(ChatMessage message);
	}
}
