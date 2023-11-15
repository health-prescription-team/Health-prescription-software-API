namespace Health_prescription_software_API.Services.Tests
{
    using Data;
    using Data.Entities.User;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

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

            var usersDbSet = new Mock<DbSet<User>>();

            var data = new User[] { }.AsQueryable();

            usersDbSet.As<IAsyncEnumerable<User>>()
              .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
              .Returns(new TestAsyncEnumerator<User>(data.GetEnumerator()));

            usersDbSet.As<IQueryable<User>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<User>(data.Provider));

            usersDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            usersDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            usersDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

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
