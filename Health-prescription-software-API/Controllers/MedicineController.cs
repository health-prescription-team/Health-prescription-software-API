
using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;
using Microsoft.AspNetCore.Mvc;

namespace Health_prescription_software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] AddMedicineDTO model)
        {
            _medicineService.Add(model);

            return Ok("Successfully added medicine");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var medicine = await _medicineService.GetById(id);

            if (medicine == null)
            {
                return NotFound($"Item with id {id} not found.");
            }

            return Ok(medicine);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _medicineService.Delete(id);

            if (result)
            {
                return Ok("Successfully deleted medicine");
            }

            return NotFound($"Item with id {id} not found.");
        }
       
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMedicineDTO medicineToEdit)
        {
            
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
                await this._medicineService.EditByIdAsync(id, medicineToEdit);
            }
            catch (Exception) 
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
