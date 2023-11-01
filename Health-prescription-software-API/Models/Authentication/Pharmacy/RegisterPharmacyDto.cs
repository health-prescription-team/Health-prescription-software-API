namespace Health_prescription_software_API.Models.Authentication.Pharmacy
{
	using System.ComponentModel.DataAnnotations;

	public class RegisterPharmacyDto
	{
		//todo: validation attributes and async. validation.

		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		public string PharmacyName { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		[Required]
		[Phone]
		public string PhoneNumber { get; set; } = null!;
	}
}
