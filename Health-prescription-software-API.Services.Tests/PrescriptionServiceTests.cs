namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Models.Prescription;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    public class PrescriptionServiceTests
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
        public async Task AddingPrescription()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            string GpId = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205";

            var newPrescription = new AddPrescriptionDto
            {
                PatientEgn = "2222222222",
                Age = 22,
                ExpiresAt = null,
                Diagnosis = "Грип",
                PrescriptionDetails = new AddPrescriptionDetailsDto[]
                {
                    new()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = 50,
                        LunchTimeDose = 100,
                        MorningDose = 3,
                        MeasurementUnit = "ml",
                        Notes = "4343-4255=144332"
                    }
                }
            };

            var expectedPrescriptionsCount = await dbContext.Prescriptions.CountAsync() + 1;
            var expectedPrescriptionDetailsCount = await dbContext.PrescriptionDetails.CountAsync() + 1;

            // Act

            var prescriptionId = await prescriptionService.Add(newPrescription, GpId);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(prescriptionId, Is.Not.EqualTo(Guid.Empty));
                Assert.That(dbContext.Prescriptions.Count(), Is.EqualTo(expectedPrescriptionsCount));
                Assert.That(dbContext.PrescriptionDetails.Count(), Is.EqualTo(expectedPrescriptionDetailsCount));
            });
        }

        [Test]
        public async Task DeletePrescription()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1");

            var expectedPrescriptionsCount = await dbContext.Prescriptions.CountAsync() - 1;
            var expectedPrescriptionDetailsCount = await dbContext.PrescriptionDetails.CountAsync() - 2;

            // Act

            prescriptionService.Delete(prescriptionId);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(dbContext.Prescriptions.Count(), Is.EqualTo(expectedPrescriptionsCount));
                Assert.That(dbContext.PrescriptionDetails.Count(), Is.EqualTo(expectedPrescriptionDetailsCount));
            });
        }

        [Test]
        public async Task EditPrescription()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1");
            var GpId = "752140d6-b0ed-4dd9-bfc0-96cf0bc87205";

            var expectedDiagnosis = "Главобол Edited";
            var expectedNotes = "4343-4255=144332 Edited";
            var expectedEveningDose = 1111;

            EditPrescriptionDTO editPrescriptionDTO = new()
            {
                Id = prescriptionId,
                PatientEgn = "2222222222",
                Age = 22,
                Diagnosis = expectedDiagnosis,
                ExpiresAt = new DateTime(2022, 7, 17),
                PrescriptionDetails = new EditPrescriptionDetailsDTO[]
                {
                    new()
                    {
                        MedicineId = Guid.Parse("888391c6-781b-4cd4-b364-9c949baf0623"),
                        EveningDose = expectedEveningDose,
                        LunchTimeDose = 100,
                        MorningDose = 3,
                        MeasurementUnit = "ml",
                        Notes = expectedNotes
                    }
                }
            };

            // Act

            var editedPrescriptionId = await prescriptionService.Edit(editPrescriptionDTO, GpId);

            // Assert

            var editedPrescription = await dbContext.Prescriptions.FindAsync(editedPrescriptionId);
            var editedPrescriptionDetails = await dbContext.PrescriptionDetails.Where(pd => pd.PrescriptionId == editedPrescriptionId).ToListAsync();

            Assert.Multiple(() =>
            {
                Assert.That(editedPrescriptionId, Is.EqualTo(prescriptionId));
                Assert.That(editedPrescription!.Diagnosis, Is.EqualTo(expectedDiagnosis));
                Assert.That(editedPrescriptionDetails[0].EveningDose, Is.EqualTo(expectedEveningDose));
                Assert.That(editedPrescriptionDetails[0].Notes, Is.EqualTo(expectedNotes));
                Assert.That(editedPrescriptionDetails, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public async Task FinishPrescriptionExistingPrescriptionReturnsTrue()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1");

            // Act

            bool finishPrescriptionResult = await prescriptionService.FinishPrescription(prescriptionId);

            // Assert

            var fulfilledPrescription = await dbContext.Prescriptions.FindAsync(prescriptionId);

            Assert.Multiple(() =>
            {
                Assert.That(finishPrescriptionResult, Is.True);
                Assert.That(fulfilledPrescription!.IsFulfilled, Is.True);
                Assert.That(fulfilledPrescription.FulfillmentDate, Is.Not.Null);
            });
        }

        [Test]
        public async Task FinishPrescriptionNonExistingPrescriptionReturnsFalse()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var invalidPrescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c7");

            // Act

            bool finishPrescriptionResult = await prescriptionService.FinishPrescription(invalidPrescriptionId);

            // Assert

            Assert.That(finishPrescriptionResult, Is.False);
        }

        [Test]
        public async Task GetPatientPrescriptionsReturnsList()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var patientEgn = "2222222222";

            var actualPatientPrescriptions = dbContext.Prescriptions.Where(p => p.PatientEgn == patientEgn).ToArrayAsync().Result.Length;

            // Act

            var patientPrescriptions = await prescriptionService.GetPatientPrescriptions(patientEgn);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(patientPrescriptions, Is.InstanceOf<PatientPrescriptionsDTO>());
                Assert.That(patientPrescriptions.PatientPrescriptions.Count(), Is.EqualTo(actualPatientPrescriptions));
            });
        }

        [Test]
        public async Task GetPrescriptionDetails()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext);

            var prescriptionId = Guid.Parse("218537f0-2582-4d36-85f9-3f11859911c1");

            var expectedPrescriptionDetails = await dbContext.Prescriptions.FindAsync(prescriptionId);

            // Act

            var prescriptionDetails = await prescriptionService.GetPrescriptionDetails(prescriptionId);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(prescriptionDetails.PrescriptionDetails, Is.Not.Empty);
                Assert.That(prescriptionDetails.PrescriptionDetails.Count, Is.EqualTo(expectedPrescriptionDetails!.PrescriptionDetails.Count));
                Assert.That(prescriptionDetails.PatientEgn, Is.EqualTo(expectedPrescriptionDetails.PatientEgn));
            });
        }
    }
}
