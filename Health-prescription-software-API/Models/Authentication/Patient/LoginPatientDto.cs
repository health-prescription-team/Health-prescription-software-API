﻿using System.ComponentModel.DataAnnotations;

namespace Health_prescription_software_API.Models.Authentication.Patient
{
    public class LoginPatientDto
    {
        [Required]
        public string Egn { get; set; }

        [Required]
        public string Password { get; set; } = null!;
    }
}
