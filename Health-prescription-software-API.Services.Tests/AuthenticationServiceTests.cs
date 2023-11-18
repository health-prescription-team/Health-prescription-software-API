namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Data.Entities.User;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using static Utilities.MockQueryableDbSet;
    using static Seeding.UserSeed;

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
                {"Jwt:Issuer", "testIssuer" },
                {"Jwt:Audience", "testAudience" }
            };

            configuration = new Mock<IConfiguration>();

            configuration.Setup(config => config[It.IsAny<string>()]).Returns((string key) => configurationSettings.ContainsKey(key) ? configurationSettings[key] : null);

            // UserManager and SignInManager mock setup
            
            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            signInManager = new Mock<SignInManager<User>>(userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null);
        }
    }
}
