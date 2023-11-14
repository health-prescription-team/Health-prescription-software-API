namespace Health_prescription_software_API.Data
{
    using Data.Entities;
    using Data.Entities.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using static Common.Roles.RoleConstants;


    public class HealthPrescriptionDbContext : IdentityDbContext<User>
    {
        public HealthPrescriptionDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = GP,
                    NormalizedName = GP.ToUpper()
                },
                new IdentityRole
                {
                    Name = Patient,
                    NormalizedName = Patient.ToUpper()
                },
                new IdentityRole
                {
                    Name = Pharmacist,
                    NormalizedName = Pharmacist.ToUpper()
                },
                new IdentityRole
                {
                    Name = Pharmacy,
                    NormalizedName = Pharmacy.ToUpper()
                });

            base.OnModelCreating(builder);
        }
    }
}
