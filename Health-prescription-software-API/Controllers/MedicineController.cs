
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Health_prescription_software_API.Controllers
{
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
                return NotFound();
            }

            return Ok(medicine);

        }
       
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm]EditMedicineDTO medicineToEdit)
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


        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery]QueryMedicineDTO? queryModel = null)
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
