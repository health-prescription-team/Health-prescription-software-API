using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Models.Prescription;
using Microsoft.AspNetCore.Mvc;

namespace Health_prescription_software_API.Controllers
{
    [Route("api/[controller]")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]AddPrescriptionDto prescriptionModel)
        {
          var result =   _prescriptionService.Add(prescriptionModel);

            if (result is not null)
            {
                return Ok(new {Id = result.Id});
            }

            return NotFound("Resultata ne moje da bude null!");
        }
    }
}
