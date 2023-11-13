namespace Health_prescription_software_API.Models.Authentication.Pharmacy
{
	using System.ComponentModel.DataAnnotations;

	using static Common.EntityValidationConstants.Pharmacy;

	public class RegisterPharmacyDto
	{

		[Required]
		[EmailAddress]
		[StringLength(EmailMax, MinimumLength = EmailMin)]
		public string Email { get; set; } = null!;

		[Required]
		[StringLength(NameMax, MinimumLength = NameMin)]
		//todo; is it necessary async validation
		public string PharmacyName { get; set; } = null!;

		[Required]
		// todo: is it validated on the password options?
		public string Password { get; set; } = null!;

		[Required]
		[Phone]
		// todo: is it validated on the password options?
		public string PhoneNumber { get; set; } = null!;
	}
}
