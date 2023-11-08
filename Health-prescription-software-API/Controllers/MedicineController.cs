namespace Health_prescription_software_API.Controllers
{
    using Contracts;
    using Models.Medicine;
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
    using static Common.Roles.RoleConstants;

	[Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            this.medicineService = medicineService;
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
            //todo: validate the queryModel
            try
            {
                AllMedicineDTO[] model = await medicineService.GetAllAsync(queryModel);
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
