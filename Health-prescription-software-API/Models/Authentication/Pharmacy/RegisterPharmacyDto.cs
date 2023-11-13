namespace Health_prescription_software_API.Models.Authentication.Pharmacy
{
	using System.ComponentModel.DataAnnotations;

	using static Common.EntityValidationConstants.User;

	public class RegisterPharmacyDto
	{

		[Required]
		[EmailAddress]
		[StringLength(EmailMax, MinimumLength = EmailMin)]
		public string Email { get; set; } = null!;

		[Required]
		[StringLength(PharmacyNameMaxLength, MinimumLength = PharmacyNameMinLength)]
		public string PharmacyName { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		[Required]
		[Phone]
		[RegularExpression(PhoneNumberRegexPattern, ErrorMessage = InvalidPhoneNumberErrorMessage)]
		public string PhoneNumber { get; set; } = null!;
	}
}
