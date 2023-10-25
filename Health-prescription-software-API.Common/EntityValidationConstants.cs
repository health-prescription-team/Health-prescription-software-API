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
		}

	}
}