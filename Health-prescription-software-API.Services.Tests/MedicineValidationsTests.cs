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

        [Test]
        public async Task IsPharmacyMedicineOwnerReturnsTrueWithActualOwner()
        {
            // Arrange

            var validationService = new ValidationMedicine(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");
            var pharmacyId = "31bcded4-bbe9-45a3-b235-2d24766cfdf3";

            // Act

            bool actualResult = await validationService.IsPharmacyMedicineOwner(pharmacyId, medicineId);

            // Assert

            Assert.That(actualResult, Is.True);
        }

        [Test]
        public async Task IsPharmacyMedicineOwnerReturnsTrueWithNonOwnerId()
        {
            // Arrange

            var validationService = new ValidationMedicine(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");
            var pharmacyId = "bb02a027-408e-49d7-86d3-8e7b5471feba";

            // Act

            bool actualResult = await validationService.IsPharmacyMedicineOwner(pharmacyId, medicineId);

            // Assert

            Assert.That(actualResult, Is.False);
        }
    }
}
