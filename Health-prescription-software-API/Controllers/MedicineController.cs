using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Data.Entities;
using Health_prescription_software_API.Models.Medicine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Health_prescription_software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _mediicineService;

        public MedicineController(IMedicineService mediicineService)
        {
            _mediicineService = mediicineService;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm]AddMedicineDTO model)
        {
            _mediicineService.Add(model);

            return Ok("Successfully added medicine");
        }
    }
}
