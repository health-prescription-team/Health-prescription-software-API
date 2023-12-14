namespace Health_prescription_software_API.Data.Entities
{
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

    public class UserMedicine
	{

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(Medicine))]
        public Guid MedicineId { get; set; }

        public User.User User { get; set; } = null!;

		public Medicine Medicine { get; set; } = null!;

        [Required]
        public double MedicinePrice { get; set; }
    }
}
