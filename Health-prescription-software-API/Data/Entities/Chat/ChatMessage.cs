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
        [StringLength(500, MinimumLength = 1)]
        public string Message { get; set; } = null!;

        [Column(TypeName = "Timestamp")]
        public DateTime MessageTime { get; set; }

        [Required]
        [ForeignKey(nameof(AuthorId))]
        public string AuthorId { get; set; } = null!;
        public virtual User Author { get; set; } = null!;

        [DefaultValue(false)]
        public bool IsRead { get; set; }
    }
}
