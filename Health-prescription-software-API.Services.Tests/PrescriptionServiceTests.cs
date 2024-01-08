namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Data.Entities;
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


        }
    }
}
