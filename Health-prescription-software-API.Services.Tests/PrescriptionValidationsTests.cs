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

        [Test]
        public async Task IsDeletePrescriptionValidWithValidIdReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1");

            // Act

            bool actualResult = await validationService.IsDeletePrescriptionValid(prescriptionId);

            // Assert

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public async Task IsDeletePrescriptionValidWithNonExistingIdReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c5");

            // Act

            bool actualResult = await validationService.IsDeletePrescriptionValid(prescriptionId);

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("Id"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(PrescriptionDoesNotExist));
            });
        }

        [Test]
        public async Task IsDeletePrescriptionValidWithExistingFulfilledPrescriptionReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            var prescriptionId = Guid.Parse("cb015cdd-c17a-4eb0-8de7-0a53ca017037");

            // Act

            bool actualResult = await validationService.IsDeletePrescriptionValid(prescriptionId);

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("IsFulfilled"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(CantDeletePrescription));
            });
        }

        [Test]
        public async Task IsEditPrescriptionValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            EditPrescriptionDTO editModel = new()
            {
                Id = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                Age = 44,
                PatientEgn = "3333333333",
                ExpiresAt = DateTime.Now,
                Diagnosis = "Edited",
                PrescriptionDetails = new[]
                {
                    new EditPrescriptionDetailsDTO()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 1,
                        LunchTimeDose = 1,
                        MorningDose = 1,
                        MeasurementUnit = "mg",
                        Notes = "Няма"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsEditPrescriptionValid(editModel);

            // Assert

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public async Task IsEditPrescriptionValidWithNonExistentIdReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            EditPrescriptionDTO editModel = new()
            {
                Id = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b61"),
                Age = 44,
                PatientEgn = "3333333333",
                ExpiresAt = DateTime.Now,
                Diagnosis = "Edited",
                PrescriptionDetails = new[]
                {
                    new EditPrescriptionDetailsDTO()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 1,
                        LunchTimeDose = 1,
                        MorningDose = 1,
                        MeasurementUnit = "mg",
                        Notes = "Няма"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsEditPrescriptionValid(editModel);

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("Id"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(PrescriptionDoesNotExist));
            });
        }

        [Test]
        public async Task IsEditPrescriptionValidWithExistingFulfilledPrescriptionReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            EditPrescriptionDTO editModel = new()
            {
                Id = Guid.Parse("cb015cdd-c17a-4eb0-8de7-0a53ca017037"),
                Age = 44,
                PatientEgn = "3333333333",
                ExpiresAt = DateTime.Now,
                Diagnosis = "Edited",
                PrescriptionDetails = new[]
                {
                    new EditPrescriptionDetailsDTO()
                    {
                        MedicineId = Guid.Parse("8f35d73d-6e00-4ba8-a8c8-9fc056d724ad"),
                        EveningDose = 1,
                        LunchTimeDose = 1,
                        MorningDose = 1,
                        MeasurementUnit = "mg",
                        Notes = "Няма"
                    }
                }
            };

            // Act

            bool actualResult = await validationService.IsEditPrescriptionValid(editModel);

            var modelErrorsCount = validationService.ModelErrors.Count;

            var actualModelErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualModelErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.False);
                Assert.That(modelErrorsCount, Is.EqualTo(1));
                Assert.That(actualModelErrorPropName, Is.EqualTo("IsFulfilled"));
                Assert.That(actualModelErrorMessage, Is.EqualTo(CantEditPrescription));
            });
        }

        [Test]
        public async Task IsEditPrescriptionValidReturnsFalseWithEmptyDetails()
        {
            // Arrange

            var validationService = new ValidationPrescription(dbContext);

            EditPrescriptionDTO editModel = new()
            {
                Id = Guid.Parse("203168cc-aa64-478c-86c5-7a87dcee6b6e"),
                Age = 44,
                PatientEgn = "3333333333",
                ExpiresAt = DateTime.Now,
                Diagnosis = "Edited",
                PrescriptionDetails = Array.Empty<EditPrescriptionDetailsDTO>()
            };

            // Act

            bool actualResult = await validationService.IsEditPrescriptionValid(editModel);

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
    }
}
