
namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Health_prescription_software_API.Models.Authentification;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register/GP")]
        public async Task<IActionResult> RegisterGP( GPDto GpUser)
        {
            var token = await _authenticationService.RegisterDoctor(GpUser);

            if (token == null) 
            {
                throw new ArgumentException("Failed to register a doctor");

                
            }

            return Ok(new {Token = token});
        }
    }
}
