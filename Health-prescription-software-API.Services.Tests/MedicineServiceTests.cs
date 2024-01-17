namespace Health_prescription_software_API.Services.Tests
{
    using Microsoft.AspNetCore.Http;

    using Data;
    using Models.Medicine;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    public class MedicineServiceTests
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
        public async Task AddingMedicineReturnsId()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            var creatorId = "31bcded4-bbe9-45a3-b235-2d24766cfdf3";

            AddMedicineDTO addModel = new()
            {
                Name = "Лекарство",
                MedicineCompany = "Производител",
                MedicineImage = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                MedicineDetails = "Лекарство за болни.",
                Ingredients = "Химикали 10гр., Още химикали 20гр.",
                Price = 22.22m
            };

            var expectedMedicineCount = await dbContext.Medicines.CountAsync() + 1;

            // Act

            var addedMedicineId = await medicineService.Add(addModel, creatorId);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(addedMedicineId, Is.Not.EqualTo(Guid.Empty));
                Assert.That(dbContext.Medicines.Count(), Is.EqualTo(expectedMedicineCount));
            });
        }

        [Test]
        public async Task GetByIdReturnsMedicineDetailsDTO()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");

            var expectedName = "Аспирин";
            var expectedCompany = "Fake Pharma";
            var expectedDetails = "Аспирин 50 мг.";
            var expectedPrice = 12.33m;
            var expectedIngredients = "Кора от дърво?";

            // Act

            MedicineDetailsDTO medicineDetailsDto = await medicineService.GetById(medicineId);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(medicineDetailsDto.Name, Is.EqualTo(expectedName));
                Assert.That(medicineDetailsDto.MedicineCompany, Is.EqualTo(expectedCompany));
                Assert.That(medicineDetailsDto.MedicineDetails, Is.EqualTo(expectedDetails));
                Assert.That(medicineDetailsDto.Price, Is.EqualTo(expectedPrice));
                Assert.That(medicineDetailsDto.Ingredients, Is.EqualTo(expectedIngredients));
            });
        }

        [Test]
        public async Task EditByIdReturnsEditedMedicineIdWithNullImage()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");

            EditMedicineDTO editMedicineDTO = new()
            {
                Name = "Аспирин Edited",
                MedicineImage = null,
                Price = 1000,
                MedicineCompany = "Edited",
                MedicineDetails = "Edited Details",
                Ingredients = "Ingredients Edited"
            };

            // Act

            var editedMedicineId = await medicineService.EditByIdAsync(medicineId, editMedicineDTO);

            // Assert

            var editedEntity = await dbContext.Medicines.FindAsync(medicineId);

            Assert.Multiple(() =>
            {
                Assert.That(editedMedicineId, Is.EqualTo(medicineId));
                Assert.That(editedEntity!.Name, Is.EqualTo(editMedicineDTO.Name));
                Assert.That(editedEntity!.Price, Is.EqualTo(editMedicineDTO.Price));
                Assert.That(editedEntity!.MedicineCompany, Is.EqualTo(editMedicineDTO.MedicineCompany));
                Assert.That(editedEntity!.MedicineDetails, Is.EqualTo(editMedicineDTO.MedicineDetails));
                Assert.That(editedEntity!.Ingredients, Is.EqualTo(editMedicineDTO.Ingredients));
            });
        }

        [Test]
        public async Task EditByIdReturnsEditedMedicineIdWithImage()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");

            EditMedicineDTO editMedicineDTO = new()
            {
                Name = "Аспирин Edited",
                MedicineImage = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Price = 1000,
                MedicineCompany = "Edited",
                MedicineDetails = "Edited Details",
                Ingredients = "Ingredients Edited"
            };

            // Act

            var editedMedicineId = await medicineService.EditByIdAsync(medicineId, editMedicineDTO);

            // Assert

            var editedEntity = await dbContext.Medicines.FindAsync(medicineId);

            Assert.Multiple(() =>
            {
                Assert.That(editedMedicineId, Is.EqualTo(medicineId));
                Assert.That(editedEntity!.Name, Is.EqualTo(editMedicineDTO.Name));
                Assert.That(editedEntity!.Price, Is.EqualTo(editMedicineDTO.Price));
                Assert.That(editedEntity!.MedicineCompany, Is.EqualTo(editMedicineDTO.MedicineCompany));
                Assert.That(editedEntity!.MedicineDetails, Is.EqualTo(editMedicineDTO.MedicineDetails));
                Assert.That(editedEntity!.Ingredients, Is.EqualTo(editMedicineDTO.Ingredients));
            });
        }

        [Test]
        public async Task GetAllAsyncWithNullQuery()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            QueryMedicineDTO? medicineQuery = null;

            // Act

            var medicines = await medicineService.GetAllAsync(medicineQuery);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(medicines.Medicines, Is.Not.Empty);
                Assert.That(medicines.MedicinesCount, Is.EqualTo(dbContext.Medicines.Count()));
            });
        }

        [Test]
        public async Task GetAllAsyncWithSearchTermOnly()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            QueryMedicineDTO? medicineQuery = new ()
            {
                SearchTerm = "Асп"
            };

            // Act

            var medicines = await medicineService.GetAllAsync(medicineQuery);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(medicines.Medicines, Is.Not.Empty);
                Assert.That(medicines.MedicinesCount, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task GetAllAsyncWithEntriesPerPage()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            QueryMedicineDTO? medicineQuery = new()
            {
                SearchTerm = "А",
                EntriesPerPage = 1,
                PageNumber = 1
            };

            // Act

            var medicines = await medicineService.GetAllAsync(medicineQuery);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(medicines.Medicines, Is.Not.Empty);
                Assert.That(medicines.MedicinesCount, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetAllMinimal()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            // Act

            var allMedicinesMinimal = await medicineService.GetAllMinimalAsync();

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(allMedicinesMinimal, Is.Not.Empty);
                Assert.That(allMedicinesMinimal.Count(), Is.EqualTo(3));
            });
        }

        [Test]
        public async Task DeleteMedicine()
        {
            // Arrange

            var medicineService = new MedicineService(dbContext);

            var medicineId = Guid.Parse("b7540da6-da0f-40b0-bd8c-259e42e3af8d");

            // Act

            await medicineService.Delete(medicineId);

            // Assert

            Assert.That(dbContext.Medicines.Count(), Is.EqualTo(2));
        }
    }
}
