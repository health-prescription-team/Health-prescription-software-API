namespace Health_prescription_software_API.Controllers;

using Health_prescription_software_API.Contracts;
using Health_prescription_software_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MedicineController : ControllerBase
{
    private IMedicineService _medicineService;

    public MedicineController(MedicineService medicineService)
    {
        _medicineService = medicineService;
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
