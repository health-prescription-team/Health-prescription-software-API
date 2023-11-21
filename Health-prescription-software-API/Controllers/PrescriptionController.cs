using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Contracts.Validations;
using Health_prescription_software_API.Models.Prescription;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Health_prescription_software_API.Controllers
{

    [Route("api/[controller]")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IValidaitonPrescription _validationService;

        private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

        public PrescriptionController(IPrescriptionService prescriptionService,
            IValidaitonPrescription validationService, 
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _prescriptionService = prescriptionService;
            this._validationService = validationService;
            this.apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]AddPrescriptionDto prescriptionModel)
        {


            try
            {
                var checkPatientEgn = await _validationService.IsPrescriptionValid(prescriptionModel);

                if (checkPatientEgn == false)
                {
                    foreach(var error in _validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName, error.ErrorMessage);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var hashedPrescriptionId = await _prescriptionService.Add(prescriptionModel);
                
                 return Ok(new {PrescriptionId = hashedPrescriptionId});

            }
            catch (Exception)
            {

                return StatusCode(500);
            }

        }
    }
	

}
