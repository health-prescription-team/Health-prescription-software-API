namespace Health_prescription_software_API.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    using static Common.EntityValidationConstants.User;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Prescription
    {
        public Prescription()
        {
            PrescriptionDetails = new HashSet<PrescriptionDetails>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string GpId { get; set; } = null!;
        [ForeignKey(nameof(GpId))]
        public virtual User.User Gp { get; set; } = null!;

        [Required]
        [RegularExpression(EgnRegexPattern, ErrorMessage = InvalidEgnErrorMessage)]
        public string PatientEgn { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        public string Diagnosis { get; set; } = null!;

        [Required]
        [DefaultValue(false)]
        public bool IsFulfilled { get; set; }

        [Column(TypeName = "Timestamp")]
        public DateTime? FulfillmentDate { get; set; }

        [Required]
        [Column(TypeName = "Timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "Timestamp")]
        public DateTime? ExpiresAt { get; set; }

        public virtual ICollection<PrescriptionDetails> PrescriptionDetails { get; set; }
    }
}
