namespace Health_prescription_software_API.Data.Entities.Chat
{
    using Health_prescription_software_API.Data.Entities.User;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChatMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Length(minimumLength: 1, maximumLength: 500)]
        public string Message { get; set; } = null!;

        public DateTime MessageTime { get; set; }

        [Required]
        [ForeignKey(nameof(SenderId))]
        public string SenderId { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;

        [DefaultValue(false)]
        public bool IsRead { get; set; }
    }
}
