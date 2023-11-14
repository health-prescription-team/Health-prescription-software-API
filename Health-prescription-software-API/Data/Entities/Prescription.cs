using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Health_prescription_software_API.Data.Entities
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PatientId { get; set; } = null!;


        [Required]
        public string Egn { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }


        [Required]
        public string GpName { get; set; } = null!;

        [Required]
        public string Diagnosis { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime EndedAt { get; set;} 
    }
}
