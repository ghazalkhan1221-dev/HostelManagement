
using HostelManagement.Application.DTOs.Resident;
using HostelManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResidentController : ControllerBase
{
    private readonly IResidentService _residentService;

    public ResidentController(IResidentService residentService)
    {
        _residentService = residentService;
    }

    // GET: api/Resident
    [HttpGet]
    public async Task<IActionResult> GetAllResidents()
    {
        var residents = await _residentService.GetAllAsync();
        return Ok(residents);
    }

    // GET: api/Resident/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetResidentById(Guid id)
    {
        var resident = await _residentService.GetByIdAsync(id);

        if (resident == null)
            return NotFound("Resident not found.");

        return Ok(resident);
    }

    // POST: api/Resident
    [HttpPost]
    public async Task<IActionResult> CreateResident(
        CreateResidentDto dto)
    {
        var resident = await _residentService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetResidentById),
            new { id = resident.Id },
            resident);
    }

    // PUT: api/Resident/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateResident(
        Guid id,
        UpdateResidentDto dto)
    {
        var updated = await _residentService.UpdateAsync(id, dto);

        if (!updated)
            return NotFound("Resident not found.");

        return Ok("Resident updated successfully.");
    }

    // PATCH: api/Resident/{id}/checkout
    [HttpPatch("{id:guid}/checkout")]
    public async Task<IActionResult> CheckoutResident(Guid id)
    {
        var result = await _residentService.CheckoutAsync(id);

        if (!result)
            return NotFound("Resident not found.");

        return Ok("Resident checked out successfully.");
    }

    // PATCH: api/Resident/{id}/inactive
    [HttpPatch("{id:guid}/inactive")]
    public async Task<IActionResult> MarkInactive(Guid id)
    {
        var result = await _residentService.MarkInactiveAsync(id);

        if (!result)
            return NotFound("Resident not found.");

        return Ok("Resident marked inactive.");
    }

    // DELETE: api/Resident/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteResident(Guid id)
    {
        try
        {
            var result = await _residentService.DeleteAsync(id);

            if (!result)
                return NotFound("Resident not found.");

            return Ok("Resident deleted successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
