namespace Health_prescription_software_API.Models.Medicine
{
	using System.ComponentModel.DataAnnotations;

	using Microsoft.EntityFrameworkCore;

	using static Common.EntityValidationConstants.Medicine;


	public class AllMedicineDTO
	{
		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;


		[Required]
		[Range(PriceMinValue, PriceMaxValue)]
		public decimal Price { get; set; }


		[Required]
		[StringLength(CompanyMaxLength, MinimumLength = CompanyMinLength)]
		public string MedicineCompany { get; set; } = null!;
	}
}
