namespace Health_prescription_software_API.Services.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using Data;
    using Data.Entities.User;
    using Models.Authentication.Pharmacist;

    using static Utilities.MockQueryableDbSet;
    using static Seeding.UserSeed;
    using Health_prescription_software_API.Models.Authentication.Patient;
    using Health_prescription_software_API.Models.Authentication.GP;

    public class AuthenticationServiceTests
    {
        private Mock<HealthPrescriptionDbContext> dbContext;
        private Mock<IConfiguration> configuration;
        private Mock<UserManager<User>> userManager;
        private Mock<SignInManager<User>> signInManager;

        [SetUp]
        public void Setup()
        {
            // Database mock setup

            var usersDbSet = MockDbSet(GenerateUsers());

            dbContext = new Mock<HealthPrescriptionDbContext>(new DbContextOptions<DbContext>());

            dbContext.Setup(m => m.Users).Returns(usersDbSet.Object);

            // IConfiguration mock setup

            var configurationSettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "erhgrRTGsadfef545erdgdfgr6tDFHGR4653fgdggsdf" },
                {"Jwt:Issuer", "testIssuer" },
                {"Jwt:Audience", "testAudience" }
            };

            configuration = new Mock<IConfiguration>();

            configuration.Setup(config => config[It.IsAny<string>()]).Returns((string key) => configurationSettings.ContainsKey(key) ? configurationSettings[key] : null);

            // UserManager and SignInManager mock setup

            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            signInManager = new Mock<SignInManager<User>>(
                userManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                null,
                null,
                null);
        }

        [Test]
        public async Task RegisterPharmacistWithValidData()
        {
            // Arrange
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);
            
            PatientDto formModel = new()
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
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

            PatientDto formModel = new()
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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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
            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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

            var authService = new AuthenticationService(dbContext.Object, configuration.Object, userManager.Object, signInManager.Object);

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
    }
}
