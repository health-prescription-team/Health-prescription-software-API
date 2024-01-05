namespace Health_prescription_software_API.Services.Tests
{
    using Microsoft.AspNetCore.Identity;

    using Data;
    using Common.Roles;
    using Data.Entities.User;
    using Models.Authentication.Pharmacy;

    using ValidationServices;

    using static Utilities.MockQueryableDbSet;
    using static Seeding.UserSeed;

    using static Common.EntityValidationErrorMessages.User;
    using static Common.EntityValidationErrorMessages.Authentication;


    public class AuthenticationValidationsTests
    {
        private Mock<HealthPrescriptionDbContext> dbContext;
        private Mock<DbSet<User>> usersDbSet;
        private Mock<UserManager<User>> userManager;

        [SetUp]
        public void Setup()
        {
            dbContext = new Mock<HealthPrescriptionDbContext>(new DbContextOptions<DbContext>());

            usersDbSet = MockDbSet(GenerateUsers());

            dbContext.Setup(m => m.Users).Returns(usersDbSet.Object);

            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [Test]
        public async Task IsPharmacyLoginValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext.Object, userManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "pharmacy@test.bg",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacy).Result).Returns(true);

            // Act

            bool validationResult = await validationService.IsPharmacyLoginValid(loginModel);

            // Assert

            Assert.That(validationResult, Is.True);
            Assert.That(actual: validationService.ModelErrors.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task IsPharmacyLoginValidWithValidDataAndWrongRoleReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext.Object, userManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "pharmacy@test.bg",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacy).Result).Returns(false);

            var expectedErrorPropName = RoleConstants.Pharmacy;
            var expectedErrorMessage = InvalidLogin;

            // Act

            var validationResult = await validationService.IsPharmacyLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.That(validationResult, Is.False);
            Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
            Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public async Task IsPharmacyLoginValidNonExistingEmailReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext.Object, userManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "nonExisting@test.bg",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacy).Result).Returns(true);

            var expectedErrorPropName = nameof(loginModel.Email);
            var expectedErrorMessage = PharmacyUserWithEmailDoesNotExists;

            // Act

            var validationResult = await validationService.IsPharmacyLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.That(validationResult, Is.False);
            Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
            Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
        }
    }
}
