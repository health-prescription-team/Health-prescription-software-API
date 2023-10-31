using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentication.GP
{
    public class RegisterGpDto
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string MiddleName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public int Egn { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; } = null!;

        [Required]
        public int UinNumber { get; set; }

        [Required]
        public string HospitalName { get; set; } = null!;

		[Required]
        public string Password { get; set; } = null!;

		[Required]
        public string PhoneNumber { get; set; } = null!;
	}
}
