using Microsoft.AspNetCore.Mvc;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HostelController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllHostels()
    {
        return Ok(new
        {
            Message = "Hostel API Working Successfully",
            Project = "Hostel Management System"
        });
    }
}