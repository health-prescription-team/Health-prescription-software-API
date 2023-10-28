using Microsoft.AspNetCore.Identity;

namespace Health_prescription_software_API.Data.Entities.User;

public class User:IdentityUser
{

   public string? FirstName { get; set; } = null!;
   public string? MiddleName { get; set; }
   public  string? LastName { get; set; }
   
   public int? Egn { get; set; }
   
   public byte[] ProfilePicture { get; set; }
   
   public int? UinNumber { get; set; }

   public string? HostpitalName { get; set; }
   
   public string? PharmacyName { get; set; }

}