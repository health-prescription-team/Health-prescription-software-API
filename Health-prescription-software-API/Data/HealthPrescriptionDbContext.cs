
namespace Health_prescription_software_API.Data
{
    using Data.Entities;
    using Data.Entities.Chat;
    using Data.Entities.User;
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

        public virtual DbSet<ChatMessage> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
           

            base.OnModelCreating(builder);
            
        }
    }
}
