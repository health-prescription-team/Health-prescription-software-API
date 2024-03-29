﻿namespace Health_prescription_software_API.Services.Tests
{
    using Microsoft.AspNetCore.Identity;

    using Data;
    using Common.Roles;
    using Data.Entities.User;
    using Models.Authentication.Pharmacy;
    using Models.Authentication.Pharmacist;
    using Models.Authentication.Patient;
    using Models.Authentication.GP;

    using ValidationServices;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    using static Common.EntityValidationErrorMessages.User;
    using static Common.EntityValidationErrorMessages.Authentication;

    public class AuthenticationValidationsTests
    {
        private HealthPrescriptionDbContext dbContext;
        private Mock<UserManager<User>> userManager;

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

            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task IsPharmacyLoginValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "pharmacy@test.bg",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacy).Result).Returns(true);

            // Act

            bool validationResult = await validationService.IsPharmacyLoginValid(loginModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPharmacyLoginValidWithValidDataAndWrongRoleReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

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

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacyLoginValidNonExistingEmailReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

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

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacyRegisterValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacyDto registerModel = new()
            {
                Email = "newPharmacy@test.bg",
                PharmacyName = "Нова Аптека",
                PhoneNumber = "1234567890",
                Password = "Parola1!"
            };

            // Act

            var validationResult = await validationService.IsPharmacyRegisterValid(registerModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPharmacyRegisterValidWithExistingEmailReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacyDto registerModel = new()
            {
                Email = "pharmacy@test.bg",
                PharmacyName = "Нова Аптека",
                PhoneNumber = "1234567890",
                Password = "Parola1!"
            };

            var expectedErrorPropName = nameof(registerModel.Email);
            var expectedErrorMessage = PharmacyUserWithEmailExists;

            // Act

            var validationResult = await validationService.IsPharmacyRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacyRegisterValidWithExistingPharmacyNameReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacyDto registerModel = new()
            {
                Email = "nonexisting@test.bg",
                PharmacyName = "Аптека",
                PhoneNumber = "1234567890",
                Password = "Parola1!"
            };

            var expectedErrorPropName = nameof(registerModel.PharmacyName);
            var expectedErrorMessage = PharmacyUserWithSameNameExists;

            // Act

            var validationResult = await validationService.IsPharmacyRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacyRegisterValidWithExistingPharmacyNameAndEmailReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacyDto registerModel = new()
            {
                Email = "pharmacy@test.bg",
                PharmacyName = "Аптека",
                PhoneNumber = "1234567890",
                Password = "Parola1!"
            };

            var expectedNameErrorPropName = nameof(registerModel.PharmacyName);
            var expectedNameErrorMessage = PharmacyUserWithSameNameExists;

            var expectedEmailErrorPropName = nameof(registerModel.Email);
            var expectedEmailErrorMessage = PharmacyUserWithEmailExists;

            // Act

            var validationResult = await validationService.IsPharmacyRegisterValid(registerModel);

            var actualEmailErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualEmailErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            var actualNameErrorPropName = validationService.ModelErrors.ToArray()[1].ErrorPropName;
            var actualNameErrorMessage = validationService.ModelErrors.ToArray()[1].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(2));
                Assert.That(actualNameErrorPropName, Is.EqualTo(expectedNameErrorPropName));
                Assert.That(actualNameErrorMessage, Is.EqualTo(expectedNameErrorMessage));
                Assert.That(actualEmailErrorPropName, Is.EqualTo(expectedEmailErrorPropName));
                Assert.That(actualEmailErrorMessage, Is.EqualTo(expectedEmailErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacistRegisterValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacistDto registerModel = new()
            {
                FirstName = "Фармацевт",
                LastName = "Тестов",
                Egn = "9999999999",
                UinNumber = "9999999999",
                PhoneNumber = "0888888888",
                Email = "nov@abv.bg"
            };

            // Act

            var validationResult = await validationService.IsPharmacistRegisterValid(registerModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPharmacistRegisterValidWithExistingEmailReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacistDto registerModel = new()
            {
                FirstName = "Фармацевт",
                LastName = "Тестов",
                Egn = "9999999999",
                UinNumber = "9999999999",
                PhoneNumber = "0888888888",
                Email = "test@abv.bg"
            };

            var expectedErrorPropName = nameof(registerModel.Email);
            var expectedErrorMessage = UserWithEmailExists;

            // Act

            var validationResult = await validationService.IsPharmacistRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacistRegisterValidWithExistingEgnReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacistDto registerModel = new()
            {
                FirstName = "Фармацевт",
                LastName = "Тестов",
                Egn = "4444444444",
                UinNumber = "9999999999",
                PhoneNumber = "0888888888",
                Email = "nov@abv.bg"
            };

            var expectedErrorPropName = nameof(registerModel.Egn);
            var expectedErrorMessage = UserWithEgnExists;

            // Act

            var validationResult = await validationService.IsPharmacistRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacistRegisterValidWithExistingUinReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPharmacistDto registerModel = new()
            {
                FirstName = "Фармацевт",
                LastName = "Тестов",
                Egn = "9999999999",
                UinNumber = "4444444444",
                PhoneNumber = "0888888888",
                Email = "nov@abv.bg"
            };

            var expectedErrorPropName = nameof(registerModel.UinNumber);
            var expectedErrorMessage = UserWithUinNumberExists;

            // Act

            var validationResult = await validationService.IsPharmacistRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacistLoginValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "4444444444",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacist).Result).Returns(true);

            // Act

            bool validationResult = await validationService.IsPharmacistLoginValid(loginModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPharmacistLoginValidWithValidDataAndWrongRoleReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "4444444444",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacist).Result).Returns(false);

            var expectedErrorPropName = RoleConstants.Pharmacist;
            var expectedErrorMessage = InvalidLogin;

            // Act

            var validationResult = await validationService.IsPharmacistLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPharmacistLoginValidNonExistingUserReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext  , userManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "9999999999",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Pharmacist).Result).Returns(true);

            var expectedErrorPropName = nameof(loginModel.Egn);
            var expectedErrorMessage = UserWithEgnDoesNotExist;

            // Act

            var validationResult = await validationService.IsPharmacistLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPatientLoginValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "3333333333",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Patient).Result).Returns(true);

            // Act

            bool validationResult = await validationService.IsPatientLoginValid(loginModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPatientLoginValidWithValidDataAndWrongRoleReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "3333333333",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Patient).Result).Returns(false);

            var expectedErrorPropName = RoleConstants.Patient;
            var expectedErrorMessage = InvalidLogin;

            // Act

            var validationResult = await validationService.IsPatientLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPatientLoginValidNonExistingUserReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "9999999999",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.Patient).Result).Returns(true);

            var expectedErrorPropName = nameof(loginModel.Egn);
            var expectedErrorMessage = UserWithEgnDoesNotExist;

            // Act

            var validationResult = await validationService.IsPatientLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsPatientRegisterValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPatientDto registerModel = new()
            {
                FirstName = "Пациент",
                LastName = "Тестов",
                Egn = "999999999",
                PhoneNumber = "0888888888",
                Password = "Parola1!"
            };

            // Act

            var validationResult = await validationService.IsPatientRegisterValid(registerModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsPatientRegisterValidWithExistingEgnReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterPatientDto registerModel = new()
            {
                FirstName = "Пациент",
                LastName = "Тестов",
                Egn = "3333333333",
                PhoneNumber = "0888888888",
                Password = "Parola1!"
            };

            var expectedErrorPropName = nameof(registerModel.Egn);
            var expectedErrorMessage = UserWithEgnExists;

            // Act

            var validationResult = await validationService.IsPatientRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsGpRegisterValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterGpDto registerModel = new()
            {
                FirstName = "Доктор",
                MiddleName = "Тестов",
                LastName = "Тестов",
                Egn = "9999999999",
                UinNumber = "9999999999",
                PhoneNumber = "0888888888"
            };

            // Act

            var validationResult = await validationService.IsGpRegisterValid(registerModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsGpRegisterValidWithExistingEgnReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterGpDto registerModel = new()
            {
                FirstName = "Доктор",
                MiddleName = "Тестов",
                LastName = "Тестов",
                Egn = "5555555555",
                UinNumber = "9999999999",
                PhoneNumber = "0888888888"
            };

            var expectedErrorPropName = nameof(registerModel.Egn);
            var expectedErrorMessage = UserWithEgnExists;

            // Act

            var validationResult = await validationService.IsGpRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsGpRegisterValidWithExistingUinReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            RegisterGpDto registerModel = new()
            {
                FirstName = "Доктор",
                MiddleName = "Тестов",
                LastName = "Тестов",
                Egn = "9999999999",
                UinNumber = "5555555555",
                PhoneNumber = "0888888888"
            };

            var expectedErrorPropName = nameof(registerModel.UinNumber);
            var expectedErrorMessage = UserWithUinNumberExists;

            // Act

            var validationResult = await validationService.IsGpRegisterValid(registerModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsGpLoginValidWithValidDataReturnsTrue()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "5555555555",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.GP).Result).Returns(true);

            // Act

            bool validationResult = await validationService.IsGpLoginValid(loginModel);

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.True);
                Assert.That(validationService.ModelErrors, Is.Empty);
            });
        }

        [Test]
        public async Task IsGpLoginValidWithValidDataAndWrongRoleReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "5555555555",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.GP).Result).Returns(false);

            var expectedErrorPropName = RoleConstants.GP;
            var expectedErrorMessage = InvalidLogin;

            // Act

            var validationResult = await validationService.IsGpLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }

        [Test]
        public async Task IsGpLoginValidNonExistingUserReturnsFalse()
        {
            // Arrange

            var validationService = new ValidationAuthentication(dbContext, userManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "9999999999",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.IsInRoleAsync(It.IsAny<User>(), RoleConstants.GP).Result).Returns(true);

            var expectedErrorPropName = nameof(loginModel.Egn);
            var expectedErrorMessage = UserWithEgnDoesNotExist;

            // Act

            var validationResult = await validationService.IsGpLoginValid(loginModel);

            var actualErrorPropName = validationService.ModelErrors.ToArray()[0].ErrorPropName;
            var actualErrorMessage = validationService.ModelErrors.ToArray()[0].ErrorMessage;

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(validationResult, Is.False);
                Assert.That(validationService.ModelErrors, Has.Count.EqualTo(1));
                Assert.That(actualErrorPropName, Is.EqualTo(expectedErrorPropName));
                Assert.That(actualErrorMessage, Is.EqualTo(expectedErrorMessage));
            });
        }
    }
}
