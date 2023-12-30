namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Health_prescription_software_API.Contracts.Validations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models.Medicine;
    using System.Security.Claims;
    using static Common.Roles.RoleConstants;

    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService medicineService;
        private readonly IValidationMedicine validationMedicine;

        private readonly IOptions<ApiBehaviorOptions> apiBehaviorOptions;

		public MedicineController(
			IMedicineService medicineService,
			IValidationMedicine validationMedicine,
			IOptions<ApiBehaviorOptions> apiBehaviorOptions)
		{
			this.medicineService = medicineService;
			this.validationMedicine = validationMedicine;
			this.apiBehaviorOptions = apiBehaviorOptions;
		}

		[HttpPost]
        [Authorize(Roles = Pharmacy)]
        public async Task<IActionResult> Add([FromForm] AddMedicineDTO model)
        {
            try
            {
                var pharmacyId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                Guid medicineId = await medicineService.Add(model, pharmacyId);

                return Ok(new { MedicineId = medicineId });
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
                if (!await validationMedicine.IsMedicineValid(id))
                {
                    foreach (var error in validationMedicine.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var medicine = await medicineService.GetById(id);

                return Ok(medicine);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = Pharmacy)]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await medicineService.Delete(id);

            if (result)
            {
                return Ok("Successfully deleted medicine");
            }

            return NotFound($"Item with id {id} not found.");
        }


        [HttpPut("{id}")]
        [Authorize(Roles = Pharmacy)]
        public async Task<IActionResult> Edit(Guid id, [FromForm] EditMedicineDTO model)
        {
            try
            {
                var pharmacyId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                bool isMedicineIdValid = await validationMedicine.IsMedicineValid(id);

                if (isMedicineIdValid && !await validationMedicine.IsPharmacyMedicineOwner(pharmacyId, id))
                {
                    return Unauthorized();
                }

                if (!isMedicineIdValid)
                {
                    foreach (var error in validationMedicine.ModelErrors)
                    {
                        ModelState.AddModelError(error.ErrorPropName!, error.ErrorMessage!);
                    }

                    return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
                }

                var medicineId = await this.medicineService.EditByIdAsync(id, model);

                return Ok(new { MedicineId = medicineId });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> All([FromQuery] QueryMedicineDTO? queryModel = null)
        {
            if (!(await validationMedicine.IsQueryValid(queryModel)))
            {
                foreach (var error in validationMedicine.ModelErrors)
                {
                    ModelState.AddModelError(
                        error.ErrorMessage ?? string.Empty,
                        error.ErrorPropName ?? string.Empty);
                }

                return apiBehaviorOptions
                    .Value.InvalidModelStateResponseFactory(ControllerContext);
			}
            try
            {
                AllMedicineServiceModel model = await medicineService.GetAllAsync(queryModel);
                return Ok(model);
            }
            catch (Exception)
            {
                //todo: ask FE
                return StatusCode(500);
            }


        }

        [HttpGet("DynamicSearch")]
        [Authorize]
        public async Task<IActionResult> AllMinimal()
        {
            var medicaments = await medicineService.GetAllMinimalAsync();

            return Ok(new { Medicaments = medicaments });
        }

    }
}
