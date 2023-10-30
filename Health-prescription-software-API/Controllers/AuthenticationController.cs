
namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Health_prescription_software_API.Models.Authentication.GP;
	using Health_prescription_software_API.Models.Authentication.Pharmacy;
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

        [HttpPost("Register/Gp")]
        public async Task<IActionResult> RegisterGP([FromForm]RegisterGpDto GpUser)
        {
            var token = await _authenticationService.RegisterGp(GpUser);

            if (token == null) 
            {
                //todo: no need to fail. return 
                throw new ArgumentException("Failed to register a GP");

                
            }

            return Ok(new {Token = token});
        }

        [HttpPost("Login/Gp")]
        public async Task<IActionResult> LoginGp([FromForm]LoginGpDto GpUser)
        {

            var token = await _authenticationService.LoginGp(GpUser);

            if (token == string.Empty)
            {
                throw new ArgumentException("Failed to login a GP");


            }

            return Ok(new { Token = token });

        }


        [HttpPost("Register/Pharmacy")]
        public async Task<IActionResult> RegisterPharmacy([FromForm] RegisterPharmacyDto PharmacyUser)
        {
            //todo: check model state and async. validate

            var token = await _authenticationService.RegisterPharmacy(PharmacyUser);

            if (token == null)
            {
                return BadRequest();//todo: return more info.
            }

			return Ok(new { Token = token });
		}
    }
}
