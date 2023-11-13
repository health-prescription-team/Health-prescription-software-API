namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Models.Medicine;
    using static Common.Roles.RoleConstants;
    using static Common.EntityValidationErrorMessages.Medicine;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Options;
    using Health_prescription_software_API.Contracts.Validations;

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
        public async Task<IActionResult> Add([FromForm] AddMedicineDTO model)
        {
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
        //[Authorize(Roles = Pharmacy)]
        //[Authorize]
        public async Task<IActionResult> All([FromQuery] QueryMedicineDTO? queryModel = null)
        {
            if (!(await validationMedicine.IsQueryValid(queryModel)))
            {
                ModelState.AddModelError(
                    validationMedicine.ErrorPropName??string.Empty
                    , validationMedicine.ErrorMessage??string.Empty);

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

    }
}
