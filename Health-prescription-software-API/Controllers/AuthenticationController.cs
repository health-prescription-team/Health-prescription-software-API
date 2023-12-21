namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Health_prescription_software_API.Models.Authentication.GP;
    using Health_prescription_software_API.Models.Authentication.Pharmacy;
    using Health_prescription_software_API.Models.Authentication.Patient;
    using Health_prescription_software_API.Models.Authentication.Pharmacist;
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Options;
	using Health_prescription_software_API.Services.ValidationServices;
	using Health_prescription_software_API.Contracts.Validations;

	[ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationAuthentication validationService;

		private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

		public AuthenticationController(
			IAuthenticationService authenticationService
			, IOptions<ApiBehaviorOptions> apiBehaviorOptions
            , IValidationAuthentication validationService)
		{
			_authenticationService = authenticationService;
			this.apiBehaviorOptions = apiBehaviorOptions;
			this.validationService = validationService;
		}

		[HttpPost("Register/Gp")]
        public async Task<IActionResult> RegisterGP([FromForm] RegisterGpDto GpUser)
        {
            var token = await _authenticationService.RegisterGp(GpUser);

            if (token == null)
            {
                //todo: no need to fail. return 
                throw new ArgumentException("Failed to register a GP");


            }

            return Ok(new { Token = token });
        }
        [HttpPost("Login/Patient")]
        public async Task<IActionResult> LoginPatient([FromForm] LoginPatientDto PatientUser)
        {
            try
            {
                if (!await validationService.IsPatientLoginValid(PatientUser))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.LoginPatient(PatientUser);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Login failed."); //todo: return more info.
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("Register/Patient")]
        public async Task<IActionResult> RegisterPatient([FromForm] PatientDto PatientUser)
        {

            try
            {
                if (!await validationService.IsPatientRegisterValid(PatientUser))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.RegisterPatient(PatientUser);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Registration failed."); //todo: return more info.
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login/Gp")]
        public async Task<IActionResult> LoginGp([FromForm] LoginGpDto GpUser)
        {

            var token = await _authenticationService.LoginGp(GpUser);

            if (token == string.Empty)
            {
                //it's controller. instead return bad request
                throw new ArgumentException("Failed to login a GP");
            }

            return Ok(new { Token = token });
        }


        [HttpPost("Register/Pharmacy")]
        public async Task<IActionResult> RegisterPharmacy([FromForm] RegisterPharmacyDto pharmacyUser)
        {
            try
            {
                if (!await validationService.IsPharmacyRegisterValid(pharmacyUser))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.RegisterPharmacy(pharmacyUser);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Registration failed."); //todo: return more info.
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // TODO How to handle this?
                return StatusCode(500, ex.Message);
            }



        }


        [HttpPost("Login/Pharmacy")]
        public async Task<IActionResult> LoginPharmacy([FromForm] LoginPharmacyDto PharmacyUser)
        {
            try
            {
                if (!await validationService.IsPharmacyLoginValid(PharmacyUser))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.LoginPharmacy(PharmacyUser);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Login failed."); //todo: return more info.
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPost("Register/Pharmacist")]
        public async Task<IActionResult> RegisterPharmacist([FromForm] RegisterPharmacistDto formModel)
        {
            try
            {
                if (!await validationService.IsPharmacistRegisterValid(formModel))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.RegisterPharmacist(formModel);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Registration failed."); //todo: return more info.
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
            try
            {
                if (!await validationService.IsPharmacistLoginValid(formModel))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(
                            error.ErrorMessage ?? string.Empty,
                            error.ErrorPropName ?? string.Empty);
                    }

                    return apiBehaviorOptions
                        .Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var token = await _authenticationService.LoginPharmacist(formModel);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest("Login failed."); //todo: return more info.
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
