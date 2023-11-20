using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Health_prescription_software_API.Models.Prescription
{
    public class AddPrescriptionDto
    {
        public int? Id { get; set; }

        [Required]
        public string? PatientId { get; set; } = null!;


        [Required]
        public string Egn { get; set; } = null!;

        [Required]
        public int Age { get; set; }

       

        [Required]
        public string GpName { get; set; } = null!;

        [Required]
        public string Diagnosis { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime EndedAt { get; set; }
    }
}
