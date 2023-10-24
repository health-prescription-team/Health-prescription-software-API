
using Health_prescription_software_API.Contracts;
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

        [HttpGet("api/[controller]/id")]
        public async Task<IActionResult> Details(int id)
        {
            var medicine = await _medicineService.GetById(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return Ok(medicine);

        }
    }
}
