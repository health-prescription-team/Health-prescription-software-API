﻿namespace Health_prescription_software_API.Models.Authentication.Pharmacy
{
	using System.ComponentModel.DataAnnotations;

	public class LoginPharmacyDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
	}
}
