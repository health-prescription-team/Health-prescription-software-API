namespace Health_prescription_software_API.Common
{
	public static class EntityValidationConstants
	{
		public static class Medicine
		{
			public const int NameMaxLength = 200;
			public const int NameMinLength = 2;

			public const int PricePrecision = 18;
			public const int PriceScale = 2;

			public const double PriceMinValue = 0.01;
			public const double PriceMaxValue = 9999999999999999.99;

			public const int CompanyMaxLength = 200;
			public const int CompanyMinLength = 2;

			public const int DetailsMaxLength = 500;
			public const int DetailsMinLength = 5;

			public const int SearchTermMin = 1;
			public const int SearchTermMax = 500;

			public const int EntriesPerPageMin = 1;
			public const int EntriesPerPageMax = 100;
		}

		public static class User
		{
			public const int NameMaxLength = 50;
			public const int NameMinLength = 2;

			public const string EgnRegexPattern = @"^\d{10}$";
			public const string InvalidEgnErrorMessage = "Invalid EGN format.";

			public const string UinRegexPattern = @"^\d{10}$";
			public const string InvalidUniErrorMessage = "Invalid Uin number format.";

			public const int HospitalNameMaxLength = 100;
			public const int HospitalNameMinLength = 3;

			public const int PharmacyNameMaxLength = 100;
			public const int PharmacyNameMinLength = 3;

			public const string PhoneNumberRegexPattern = @"^((?=(\+359|0)?\s?(\d{1,3}|\(\d{1,3}\))(\s?\d{1,3}(\s?\d{1,3}){1,2}|\d{6}|\d{4}(\s?\d{1,3}){2})$).{9,17})$";
			public const string InvalidPhoneNumberErrorMessage = "Invalid phone number.";

			public const int EmailMin = 5;
			public const int EmailMax = 100;
		}

	}
}