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
       // [Authorize(Roles = GP)]
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
       // [Authorize(Roles = Patient)]
        public async Task<IActionResult> GetAll(string patientEgn)
        {
            try
            {
                if (!await validationService.IsPatientPrescriptionsValid(patientEgn))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var patientPrescriptions = await prescriptionService.GetPatientPrescriptions(patientEgn);

                return Ok(patientPrescriptions);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
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
        
        [HttpDelete]
        public IActionResult Delete(Guid id) 
        {
            prescriptionService.Delete(id);

            return Ok("Deleted successfully");
        }

        [HttpPut]
        [Authorize(Roles = GP)]
        public async Task<IActionResult> Edit([FromBody] EditPrescriptionDTO model)
        {
            try
            {
                string GpId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                if (!await validationService.IsGpThePrescriber(GpId, model.Id))
                {
                    return Unauthorized();
                }

                if (!await validationService.IsEditPrescriptionValid(model) || !ModelState.IsValid)
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var prescriptionId = await prescriptionService.Edit(model, GpId);

                return Ok(prescriptionId);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("Fulfill/{id}")]
        [Authorize(Roles = "Pharmacy, Pharmacist")]
        public async Task<IActionResult> Fulfill(Guid id)
        {
            try
            {
                string fulfillerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                if (await validationService.IsPrescriptionFulfilled(id))
                {
                    foreach (var error in validationService.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                await prescriptionService.FulfillPrescription(id, fulfillerId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
