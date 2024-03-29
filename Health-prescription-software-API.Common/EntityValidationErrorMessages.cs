﻿namespace Health_prescription_software_API.Common
{
	using static Common.EntityValidationConstants.Chat;

	public static class EntityValidationErrorMessages
	{
		public static class Medicine
		{
			public const string InvalidQueryString = "Invalid Query String.";

			public const string InvalidMedicineId = "Invalid medicine Id.";
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

		public static class Authentication
		{
			public const string InvalidLogin = "Invalid login";
		}

		public static class Prescription
		{
			public const string InvalidAgeRangeMessage = "Age must be between {1} and {2}.";

			public const string PatientDoesNotExist = "Patient cannot be found. He/She may not be registered.";

			public const string PrescriptionDoesNotExist = "Prescription not found.";

			public const string CantEditPrescription = "Fulfilled prescriptions can't be edited.";

			public const string CantDeletePrescription = "Fulfilled prescriptions can't be deleted.";

			public const string PrescriptionDetailsAreRequired = "At least one medicine needs to be added";
        }

		public static class Chat
		{
			public const string InvalidMsgLength = "Message must be between {1} and {2} characters.";

			public const string ServerErrorMessage = "Server encountered an unexpected error. Please try again later.";
        }
	}
}
