namespace Health_prescription_software_API.Models.Medicine
{
	using System.ComponentModel.DataAnnotations;

	using Microsoft.EntityFrameworkCore;

	using static Common.EntityValidationConstants.Medicine;


	public class AllMedicineDTO
	{
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public decimal AveragePrice { get; set; }
		
		public string MedicineCompany { get; set; } = null!;

		public byte[]? MedicineImageBytes { get; set; }
	}
}
