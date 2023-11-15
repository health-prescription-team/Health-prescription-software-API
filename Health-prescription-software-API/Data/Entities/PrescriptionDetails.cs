using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Data.Entities
{
    public class PrescriptionDetails
    {
        [Key]
        public int Id { get; set; }

        public int PrescriptionId { get; set; }

        public int? MedicineId { get; set; }

        [Required]
        public int EveningDose {  get; set; }

        [Required]
        public int LunchTimeDose { get; set; }

        [Required]
        public int MorningDose { get; set; }

        public string? Notes { get; set; }
        
      
    }
}
