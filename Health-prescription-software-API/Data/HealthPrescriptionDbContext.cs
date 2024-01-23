
namespace Health_prescription_software_API.Data
{
    using Data.Entities;
    using Health_prescription_software_API.Data.Entities.Chat;
    using Health_prescription_software_API.Data.Entities.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using static Common.Roles.RoleConstants;


    public class HealthPrescriptionDbContext : IdentityDbContext<User>
    {
		public HealthPrescriptionDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Medicine> Medicines { get; set; }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        public virtual DbSet<PrescriptionDetails> PrescriptionDetails { get; set; }

        public virtual DbSet<Conversation> Conversations { get; set; }

        public virtual DbSet<ChatMessage> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1789daa2-03da-4927-bdd2-b26158614200",
                    Name = GP,
                    NormalizedName = GP.ToUpper()
                },
                new IdentityRole
                {
                    Id = "a2093fdd-44ad-466d-99dd-66d0e6dfab37",
                    Name = Patient,
                    NormalizedName = Patient.ToUpper()
                },
                new IdentityRole
                {
                    Id = "fa8d5cb4-713a-4ee2-9a61-fa316e8189a2",
                    Name = Pharmacist,
                    NormalizedName = Pharmacist.ToUpper()
                },
                new IdentityRole
                {
                    Id = "4a7e5619-8a50-4030-9d73-ece39921e0bb",
                    Name = Pharmacy,
                    NormalizedName = Pharmacy.ToUpper()
                });

            base.OnModelCreating(builder);
            
        }
    }
}
