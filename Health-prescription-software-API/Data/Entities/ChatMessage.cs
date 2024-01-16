using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Data.Entities
{
	public class ChatMessage
	{
		[Key]
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
		public string ReceiverId { get; set; } = null!;
		public string Message { get; set; } = null!;
		public DateTime Timestamp { get; set; }
	}
}
