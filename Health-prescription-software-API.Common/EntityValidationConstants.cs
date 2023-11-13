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

		public static class Pharmacy
		{
			public const int EmailMin = 5;
			public const int EmailMax = 100;
			
			public const int NameMin = 2;
			public const int NameMax = 100;
		}

	}
}