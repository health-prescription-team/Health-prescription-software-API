using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Health_prescription_software_API.Data
{
    public class HealthPrescrtionDbContext:IdentityDbContext<IdentityUser>
    {
        public HealthPrescrtionDbContext(DbContextOptions options) : base(options) { } 
        

    }
}
