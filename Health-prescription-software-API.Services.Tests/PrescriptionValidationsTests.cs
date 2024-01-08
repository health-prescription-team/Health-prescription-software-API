namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Models.Prescription;
    using Services.ValidationServices;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    using static Common.EntityValidationErrorMessages.Prescription;
    using static Common.EntityValidationErrorMessages.Medicine;

    public class PrescriptionValidationsTests
    {
        private HealthPrescriptionDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            // In memory database setup

            var options = new DbContextOptionsBuilder<HealthPrescriptionDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryHealthDB")
                .Options;

            dbContext = new HealthPrescriptionDbContext(options);

            // Seed database

            dbContext.AddRange(GenerateUsers());
            dbContext.AddRange(GeneratePrescriptions());
            dbContext.AddRange(GeneratePrescriptionDetails());
            dbContext.AddRange(GenerateMedicine());

            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task IsAddPrescriptionValidReturnsTrueWithValidData()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            AddPrescriptionDto addModel = new()
            {
                PatientEgn = "3333333333",
                Age = 55,
                ExpiresAt = null,
                Diagnosis = "Настинка",
                PrescriptionDetails = new[]
                {
                    new AddPrescriptionDetailsDto()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 5,
                        LunchTimeDose = 5,
                        MorningDose = 5,
                        MeasurementUnit = "ml",
                        Notes = "HAHAH"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsAddPrescriptionValid(addModel);

            // Assert

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public async Task IsAddPrescriptionValidReturnsFalseWithInvalidPatientEgn()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            AddPrescriptionDto addModel = new()
            {
                PatientEgn = "9999999999",
                Age = 55,
                ExpiresAt = null,
                Diagnosis = "Настинка",
                PrescriptionDetails = new[]
                {
                    new AddPrescriptionDetailsDto()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 5,
                        LunchTimeDose = 5,
                        MorningDose = 5,
                        MeasurementUnit = "ml",
                        Notes = "HAHAH"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsAddPrescriptionValid(addModel);

            // Assert

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;


            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("PatientEgn"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(PatientDoesNotExist));
            });
        }

        [Test]
        public async Task IsAddPrescriptionValidReturnsFalseWithEmptyDetails()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            AddPrescriptionDto addModel = new()
            {
                PatientEgn = "3333333333",
                Age = 55,
                ExpiresAt = null,
                Diagnosis = "Настинка"
            };

            // Act

            bool actualResult = await validationService.IsAddPrescriptionValid(addModel);

            // Assert

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;


            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("PrescriptionDetails"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(PrescriptionDetailsAreRequired));
            });
        }

        [Test]
        public async Task IsAddPrescriptionValidReturnsFalseWithNonExistingMedicine()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            AddPrescriptionDto addModel = new()
            {
                PatientEgn = "3333333333",
                Age = 55,
                ExpiresAt = null,
                Diagnosis = "Настинка",
                PrescriptionDetails = new[]
                {
                    new AddPrescriptionDetailsDto()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 5,
                        LunchTimeDose = 5,
                        MorningDose = 5,
                        MeasurementUnit = "ml",
                        Notes = "HAHAH"
                    },
                    new AddPrescriptionDetailsDto()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0628"),
                        EveningDose = 5,
                        LunchTimeDose = 5,
                        MorningDose = 5,
                        MeasurementUnit = "ml",
                        Notes = "Non existing"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsAddPrescriptionValid(addModel);

            // Assert

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;


            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("MedicineId"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(InvalidMedicineId));
            });
        }
    }
}
