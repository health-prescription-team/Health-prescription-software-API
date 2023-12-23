namespace Health_prescription_software_API.Services.Tests.Seeding
{
    using Data.Entities;

    public static class PrescriptionSeed
    {
        public static Prescription[] GeneratePrescriptions()
        {
            return
            [
                new Prescription()
                {
                   Id = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                   GpId = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205",
                   PatientEgn = "3333333333",
                   Age = 33,
                   Diagnosis = "Грип",
                   IsFulfilled = false,
                   CreatedAt = new DateTime(2023, 5, 14),
                   ExpiresAt = null
                },
                new Prescription()
                {
                    Id = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1"),
                    GpId = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205",
                    PatientEgn = "2222222222",
                    Age = 22,
                    Diagnosis = "Главобол",
                    IsFulfilled = false,
                    CreatedAt = new DateTime(2022, 6, 17),
                    ExpiresAt = new DateTime(2022, 7, 17)
                },
                new Prescription()
                {
                    Id = Guid.Parse("cb015cdd-c17a-4eb0-8de7-0a53ca017037"),
                    GpId = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205",
                    PatientEgn = "2222222222",
                    Age = 22,
                    Diagnosis = "Настинка",
                    IsFulfilled = true,
                    CreatedAt = new DateTime(2023, 9, 14),
                    ExpiresAt = null
                },
                new Prescription()
                {
                    Id = Guid.Parse("4a960e06-6d51-4249-8f3a-315ce797555d"),
                    GpId = "0fb3a33a-9796-4f4b-8349-192a93a136dc",
                    PatientEgn = "2222222222",
                    Age = 22,
                    Diagnosis = "Грип",
                    IsFulfilled = false,
                    CreatedAt = new DateTime(2023, 1, 2),
                    ExpiresAt = null
                }
            ];
        }

        public static PrescriptionDetails[] GeneratePrescriptionDetails()
        {
            return
            [
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("c89827d6-8000-4042-82fc-91e4eece4129"),
                    PrescriptionId = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                    MedicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d"),
                    EveningDose = 1,
                    LunchTimeDose = 1,
                    MorningDose = 1,
                    MeasurementUnit = "mg",
                    Notes = "Няма"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("d7a718e1-583b-4b9d-9dfd-6a563d51ee7f"),
                    PrescriptionId = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                    MedicineId = Guid.Parse("8f35d73d-6e00-4ba8-a8c8-9fc056d724ad"),
                    EveningDose = 100,
                    LunchTimeDose = 100,
                    MorningDose = 100,
                    MeasurementUnit = "mg",
                    Notes = "Има"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("0408461b-e366-486d-88af-59d41a154921"),
                    PrescriptionId = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                    MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                    EveningDose = 50,
                    LunchTimeDose = 100,
                    MorningDose = 3,
                    MeasurementUnit = "ml",
                    Notes = "4343-4255=144332"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("147c9ded-f9c9-4486-8c17-9b90781f3eb6"),
                    PrescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1"),
                    MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                    EveningDose = 50,
                    LunchTimeDose = 100,
                    MorningDose = 3,
                    MeasurementUnit = "ml",
                    Notes = "4343-4255=144332"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("7bee30f4-d6d4-41c7-8dbd-419afa6db4a5"),
                    PrescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1"),
                    MedicineId = Guid.Parse("8f35d73d-6e00-4ba8-a8c8-9fc056d724ad"),
                    EveningDose = 100,
                    LunchTimeDose = 100,
                    MorningDose = 100,
                    MeasurementUnit = "mg",
                    Notes = "Има"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("323f7b91-43dd-4016-ba13-2d0718cfb7cd"),
                    PrescriptionId = Guid.Parse("cb015cdd-c17a-4eb0-8de7-0a53ca017037"),
                    MedicineId = Guid.Parse("8f35d73d-6e00-4ba8-a8c8-9fc056d724ad"),
                    EveningDose = 100,
                    LunchTimeDose = 100,
                    MorningDose = 100,
                    MeasurementUnit = "mg",
                    Notes = "Има"
                },
                new PrescriptionDetails()
                {
                    Id = Guid.Parse("804256ef-23ac-4f6e-8f94-4edba07af62b"),
                    PrescriptionId = Guid.Parse("4a960e06-6d51-4249-8f3a-315ce797555d"),
                    MedicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d"),
                    EveningDose = 1,
                    LunchTimeDose = 1,
                    MorningDose = 1,
                    MeasurementUnit = "mg",
                    Notes = "Няма"
                },
            ];
        }
    }
}
