using System.Xml.Linq;

namespace Health_prescription_software_API.Common
{
	public static class EntityValidationErrorMessages
	{
		public static class Medicine
		{
			public const string InvalidQueryString = "Invalid Query String.";

			public const string InvalidMedicineId = "Medicine with id does not exist.";
		}

		public static class User
		{
			public const string UserWithEmailExists = "User with the same email already exists.";

			public const string UserWithEgnExists = "User with the same egn already exists.";

			public const string UserWithUinNumberExists = "User with the same uin already exists.";

			public const string UserWithEgnDoesNotExist = "User with egn does not exist.";

            public const string PharmacyUserWithEmailDoesNotExists = "Pharmacy user with this email does not exist.";

            public const string PharmacyUserWithEmailExists = "Pharmacy user with the same email already exists.";

			public const string PharmacyUserWithSameNameExists = "Pharmacy with the same name exists.";

           

        }

		public static class Prescription
		{
			public const string InvalidAgeRangeMessage = "Age must be between {1} and {2}.";

			public const string PatientDoesNotExist = "Patient cannot be found. He/She may not be registered";

			public const string PrescriptionDoesNotExist = "Prescription not found";
        }
	}
}
