using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data;
using Health_prescription_software_API.Data.Entities.User;
using Health_prescription_software_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();


builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContext<HealthPrescriptionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;

})
    .AddEntityFrameworkStores<HealthPrescriptionDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
