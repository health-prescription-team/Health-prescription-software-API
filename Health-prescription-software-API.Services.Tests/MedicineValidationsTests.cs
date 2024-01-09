namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Models.Medicine;

    using Services.ValidationServices;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    public class MedicineValidationsTests
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
        public async Task IsMedicineValidWithExistingIdReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationMedicine(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");

            // Act

            bool actualResult = await validationService.IsMedicineValid(medicineId);

            // Assert

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public async Task IsMedicineValidWithNonExistingIdReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationMedicine(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8a");

            // Act

            bool actualResult = await validationService.IsMedicineValid(medicineId);

            // Assert

            Assert.That(actualResult, Is.False);
        }
    }
}
