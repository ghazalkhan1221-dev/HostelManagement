
using HostelManagement.Application.DTOs.Hostel;
using HostelManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HostelController : ControllerBase
{
    private readonly IHostelService _hostelService;

    public HostelController(IHostelService hostelService)
    {
        _hostelService = hostelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var hostels = await _hostelService.GetAllAsync();
        return Ok(hostels);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var hostel = await _hostelService.GetByIdAsync(id);

        if (hostel == null)
            return NotFound();

        return Ok(hostel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHostelDto dto)
    {
        try
        {
            var hostel = await _hostelService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = hostel.Id },
                hostel
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, UpdateHostelDto dto)
    {
        try
        {
            var result = await _hostelService.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _hostelService.DeleteAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
