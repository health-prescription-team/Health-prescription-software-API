namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Data.Entities;
    using Models.Prescription;

    using static Utilities.MockQueryableDbSet;
    using static Seeding.PrescriptionSeed;

    public class PrescriptionServiceTests
    {
        private Mock<HealthPrescriptionDbContext> dbContext;
        private Mock<DbSet<Prescription>> prescriptionDbSet;
        private Mock<DbSet<PrescriptionDetails>> prescriptionDetailsDbSet;

        [SetUp]
        public void Setup()
        {
            // Database mock setup

            dbContext = new Mock<HealthPrescriptionDbContext>(new DbContextOptions<DbContext>());
            prescriptionDbSet = MockDbSet(GeneratePrescriptions());
            prescriptionDetailsDbSet = MockDbSet(GeneratePrescriptionDetails());

            dbContext.Setup(m => m.Prescriptions).Returns(prescriptionDbSet.Object);
            dbContext.Setup(m => m.PrescriptionDetails).Returns(prescriptionDetailsDbSet.Object);
        }

        [Test]
        public async Task AddingPrescription()
        {
            // Arrange

            var prescriptionService = new PrescriptionService(dbContext.Object);

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

            // Act
            var prescriptionId = await prescriptionService.Add(newPrescription, "752140d6-b0ed-4dd9-bfc0-96cf0bc87205");

            // Assert
            prescriptionDbSet.Verify(m => m.AddAsync(It.IsAny<Prescription>(), It.IsAny<CancellationToken>()), Times.Once());
            dbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
