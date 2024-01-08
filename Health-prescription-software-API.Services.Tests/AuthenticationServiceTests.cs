namespace Health_prescription_software_API.Services.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using Data;
    using Data.Entities.User;
    using Models.Authentication.Pharmacist;
    using Models.Authentication.Patient;
    using Models.Authentication.GP;
    using Models.Authentication.Pharmacy;

    using static Seeding.UserSeed;
    using static Seeding.PrescriptionSeed;
    using static Seeding.MedicineSeed;

    public class AuthenticationServiceTests
    {
        private HealthPrescriptionDbContext dbContext;
        private Mock<IConfiguration> configuration;
        private Mock<UserManager<User>> userManager;
        private Mock<SignInManager<User>> signInManager;

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

            // IConfiguration mock setup

            var configurationSettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "erhgrRTGsadfef545erdgdfgr6tDFHGR4653fgdggsdf" },
                {"Jwt:Issuer", "testIssuer" },
                {"Jwt:Audience", "testAudience" }
            };

            configuration = new Mock<IConfiguration>();

            configuration.Setup(config => config[It.IsAny<string>()]).Returns((string key) => configurationSettings.TryGetValue(key, out string? value) ? value : null);

            // UserManager and SignInManager mock setup

            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);

            signInManager = new Mock<SignInManager<User>>(
                userManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                null!,
                null!,
                null!);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task RegisterPharmacistWithValidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterPharmacistDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "0123456789",
                Email = "testov@tests.org",
                UinNumber = "0123456789",
                PhoneNumber = "088888888",
                PharmacyName = "Аптека",
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!",
                ConfirmPassword = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), formModel.Password).Result).Returns(IdentityResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacist" });

            // Act

            var token = await authService.RegisterPharmacist(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task RegisterPharmacistWithInvalidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterPharmacistDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "0123456789",
                Email = "testov@tests.org",
                UinNumber = "0123456789",
                PhoneNumber = "088888888",
                PharmacyName = "Аптека",
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!",
                ConfirmPassword = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act

            var token = await authService.RegisterPharmacist(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginPharmacistWithExistingUserReturnsToken()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola1!"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacist" });

            // Act

            var token = await authService.LoginPharmacist(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task LoginPharmacistWithExistingUserWrongPasswordReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), loginModel.Password, false, false)).ReturnsAsync(SignInResult.Failed);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacist" });

            // Act

            var token = await authService.LoginPharmacist(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginPharmacistWithNonExistingUserReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacistDto loginModel = new()
            {
                Egn = "9876543210",
                Password = "Parola1!"
            };

            // Act

            var token = await authService.LoginPharmacist(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task RegisterPatientWithValidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);
            
            RegisterPatientDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "0111111112",
                PhoneNumber = "088888887",
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!",                
            };
            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), formModel.Password).Result).Returns(IdentityResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Patient" });

            // Act

            var token = await authService.RegisterPatient(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }
        [Test]
        public async Task RegisterPatientWithInvalidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterPatientDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "012345678",               
                PhoneNumber = "08888888",              
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!",               
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act

            var token = await authService.RegisterPatient(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }
        [Test]
        public async Task LoginPatientWithExistingUserReturnsToken()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola1!"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Patient" });

            // Act

            var token = await authService.LoginPatient(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }
        [Test]
        public async Task LoginPatientWithExistingUserWrongPasswordReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), loginModel.Password, false, false)).ReturnsAsync(SignInResult.Failed);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Patient" });

            // Act

            var token = await authService.LoginPatient(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }
        [Test]
        public async Task LoginPatientWithNonExistingUserReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPatientDto loginModel = new()
            {
                Egn = "9876543210",
                Password = "Parola1!"
            };

            // Act

            var token = await authService.LoginPatient(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task RegisterGpWithValidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterGpDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "0111111111",
                UinNumber = "0123456789",
                PhoneNumber = "088888888",                
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), formModel.Password).Result).Returns(IdentityResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "GP" });

            // Act

            var token = await authService.RegisterGp(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task RegisterGpWithInvalidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterGpDto formModel = new()
            {
                FirstName = "Петър",
                LastName = "Тестов",
                Egn = "0123456789",                
                UinNumber = "0123456789",
                PhoneNumber = "088888888",
                ProfilePicture = new FormFile(new MemoryStream(), 0, 321654, "test", "test.png"),
                Password = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act

            var token = await authService.RegisterGp(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginGpWithExistingUserReturnsToken()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola1!"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "GP" });

            // Act

            var token = await authService.LoginGp(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task LoginGpWithExistingUserWrongPasswordReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "0123456789",
                Password = "Parola"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), loginModel.Password, false, false)).ReturnsAsync(SignInResult.Failed);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "GP" });

            // Act

            var token = await authService.LoginGp(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginGpWithNonExistingUserReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginGpDto loginModel = new()
            {
                Egn = "9876543210",
                Password = "Parola1!"
            };

            // Act

            var token = await authService.LoginGp(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        //------------------------

        [Test]
        public async Task RegisterPharmacyWithValidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterPharmacyDto formModel = new()
            {
                Email = "pharmacy@test.bg",
                PharmacyName = "Аптека",
                PhoneNumber = "0888888888",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), formModel.Password).Result).Returns(IdentityResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacy" });

            // Act

            var token = await authService.RegisterPharmacy(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task RegisterPharmacyWithInvalidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            RegisterPharmacyDto formModel = new()
            {
                Email = "pharmacy@test.bg",
                PharmacyName = "Аптека",
                PhoneNumber = "0888888888",
                Password = "Parola1!"
            };

            userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act

            var token = await authService.RegisterPharmacy(formModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginPharmacyWithExistingUserReturnsToken()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "pharmacy@test.bg",
                Password = "Parola1!"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacy" });

            // Act

            var token = await authService.LoginPharmacy(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        }

        [Test]
        public async Task LoginPharmacyWithExistingUserWrongPasswordReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "pharmacy@test.bg",
                Password = "Parola"
            };

            signInManager.Setup(m => m.PasswordSignInAsync(It.IsAny<User>(), loginModel.Password, false, false)).ReturnsAsync(SignInResult.Failed);
            userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()).Result).Returns(new List<string> { "Pharmacy" });

            // Act

            var token = await authService.LoginPharmacy(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public async Task LoginPharmacyWithNonExistingUserReturnsEmpty()
        {
            // Arrange

            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            LoginPharmacyDto loginModel = new()
            {
                Email = "wrong@test.bg",
                Password = "Parola1!"
            };

            // Act

            var token = await authService.LoginPharmacy(loginModel);

            // Assert

            Assert.That(string.IsNullOrWhiteSpace(token), Is.True);
        }

        [Test]
        public void GetUserByEgnThrows()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext, configuration.Object, userManager.Object, signInManager.Object);

            // Act & Assert

            LoginPharmacistDto loginModel = new()
            {
                Password = "Parola1!"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await authService.LoginPharmacist(loginModel);
            });
        }
    }
}
