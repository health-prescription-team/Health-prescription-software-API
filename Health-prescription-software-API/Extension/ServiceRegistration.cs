
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Contracts.Validations;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities.User;
using Health_prescription_software_API.Services;
using Health_prescription_software_API.Services.ValidationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class ServiceRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSignalR();

        AddCors(services);

        services.AddDbContext<HealthPrescriptionDbContext>(options =>
            options.UseNpgsql(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetValue<string>("ConnectionStrings:DefaultConnection")));

        AddIdentity(services);

        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(ConfigureJwtAuthentication);


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        RegisterScopedServices(services);
      
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "CORSPolicy", p =>
            {
                p.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<HealthPrescriptionDbContext>()
        .AddDefaultTokenProviders();
    }

    private static void ConfigureJwtAuthentication(JwtBearerOptions options)
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    }



    private static void RegisterScopedServices(IServiceCollection services)
    {
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IValidationMedicine, ValidationMedicine>();
        services.AddScoped<IValidationAuthentication, ValidationAuthentication>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IValidationPrescription, ValidationPrescription>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IChatValidation, ChatValidation>();

        services.AddScoped<SeedData>();
    }

   


}
