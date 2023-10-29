namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Models.Authentification.GP;
    using Models.Authentification.Pharmacist;
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

        [HttpPost("Register/Pharmacist")]
        public async Task<IActionResult> RegisterPharmacist([FromForm] RegisterPharmacistDto formModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authenticationService.RegisterPharmacist(formModel);

                if (token == null)
                {
                    throw new ArgumentException("Failed to register a pharmacist");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // TODO How to handle this?
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login/Pharmacist")]
        public async Task<IActionResult> LoginPharmacist([FromForm] LoginPharmacistDto formModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authenticationService.LoginPharmacist(formModel);

                if (token == string.Empty)
                {
                    throw new ArgumentException("Failed to login a pharmacist");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // TODO How to handle this?
                return StatusCode(500, ex.Message);
            }
        }
    }
}
