namespace Health_prescription_software_API.Controllers
{
    using Health_prescription_software_API.Contracts;
    using Health_prescription_software_API.Contracts.Validations;
    using Health_prescription_software_API.Models.Prescription;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;

    using static Common.Roles.RoleConstants;

    [Route("api/[controller]")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService prescriptionService;
        private readonly IValidationPrescription validationService;

        private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

        public PrescriptionController(IPrescriptionService prescriptionService,
            IValidationPrescription validationService,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            this.prescriptionService = prescriptionService;
            this.validationService = validationService;
            this.apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpPost]
        [Authorize(Roles = GP)]
        public async Task<IActionResult> Add([FromBody] AddPrescriptionDto prescriptionModel)
        {
            try
            {
                string GpId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var checkPatientEgn = await validationService.IsAddPrescriptionValid(prescriptionModel);

                if (checkPatientEgn == false)
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var hashedPrescriptionId = await prescriptionService.Add(prescriptionModel, GpId);

                return Ok(new { PrescriptionId = hashedPrescriptionId });

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromForm] PatientPrescriptionsFormDTO model)
        {
            try
            {
                if (!await validationService.IsPatientPrescriptionsValid(model.EGN))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var patientPrescriptions = await prescriptionService.GetPatientPrescriptions(model.EGN);

                return Ok(patientPrescriptions);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                if (!await validationService.IsPrescriptionValid(id))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var prescription = await prescriptionService.GetPrescriptionDetails(id);

                return Ok(prescription);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
