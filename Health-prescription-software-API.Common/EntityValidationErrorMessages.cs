namespace Health_prescription_software_API.Common
{
	public static class EntityValidationErrorMessages
	{
		public static class Medicine
		{
			public const string InvalidQueryString = "Invalid Query String.";
		}

		public static class User
		{
			public const string UserWithEmailExists = "User with the same email already exists.";

			public const string UserWithEgnExists = "User with the same egn already exists.";

			public const string UserWithUinNumberExists = "User with the same uin already exists.";

			public const string UserWithEgnDoesNotExist = "User with egn does not exist.";
        }
	}
}
