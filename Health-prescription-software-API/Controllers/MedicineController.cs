namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Health_prescription_software_API.Contracts.Validations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models.Medicine;
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

        //todo: pharmacy Add
        //todo: averagePrice sql calc.
        //todo: who uses these actions?

		[HttpPost]
        //[Authorize(Roles = "NoTechAdmin")]
        public async Task<IActionResult> Add([FromForm] AddMedicineDTO model)
        {
            //todo: validations async and static
            await medicineService.Add(model);

            return Ok("Successfully added medicine");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var medicine = await medicineService.GetById(id);

            if (medicine == null)
            {
                return NotFound($"Item with id {id} not found.");
            }

            return Ok(medicine);
        }

        [HttpDelete("{id}")]
		//[Authorize(Roles = "NoTechAdmin")]
		public async Task<IActionResult> Delete(int id)
        {
            bool result = await medicineService.Delete(id);

            if (result)
            {
                return Ok("Successfully deleted medicine");
            }

            return NotFound($"Item with id {id} not found.");
        }


        [HttpPut("{id}")]
		//[Authorize(Roles = "NoTechAdmin")]
		public async Task<IActionResult> Edit(int id, [FromForm] EditMedicineDTO medicineToEdit)
        {

            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                await this.medicineService.EditByIdAsync(id, medicineToEdit);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok();
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
