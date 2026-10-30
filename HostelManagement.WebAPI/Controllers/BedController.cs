
using HostelManagement.Domain.Entities;
using HostelManagement.Domain.Enums;
using HostelManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BedController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BedController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Bed
    [HttpGet]
    public async Task<IActionResult> GetAllBeds()
    {
        var beds = await _context.Beds
            .Select(b => new
            {
                b.Id,
                b.RoomId,
                b.BedNumber,
                b.Status
            })
            .ToListAsync();

        return Ok(beds);
    }

    // GET: api/Bed/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBedById(Guid id)
    {
        var bed = await _context.Beds
            .Where(b => b.Id == id)
            .Select(b => new
            {
                b.Id,
                b.RoomId,
                b.BedNumber,
                b.Status
            })
            .FirstOrDefaultAsync();

        if (bed == null)
            return NotFound("Bed not found.");

        return Ok(bed);
    }

    // POST: api/Bed
    [HttpPost]
    public async Task<IActionResult> CreateBed(
        [FromBody] BedRequest request)
    {
        var roomExists = await _context.Rooms
            .AnyAsync(r => r.Id == request.RoomId);

        if (!roomExists)
            return BadRequest("Invalid RoomId.");

        var bed = new Bed(request.RoomId, request.BedNumber);

        _context.Beds.Add(bed);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBedById),
            new { id = bed.Id },
            new
            {
                bed.Id,
                bed.RoomId,
                bed.BedNumber,
                bed.Status
            });
    }

    // PUT: api/Bed/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBed(
        Guid id,
        [FromBody] BedUpdateRequest request)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return NotFound("Bed not found.");

        if (string.IsNullOrWhiteSpace(request.BedNumber))
            return BadRequest("Bed number is required.");

        bed.UpdateBedNumber(request.BedNumber);

        await _context.SaveChangesAsync();

        return Ok("Bed updated successfully.");
    }

    // PATCH: api/Bed/{id}/status
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateBedStatus(
        Guid id,
        [FromBody] BedStatusRequest request)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return NotFound("Bed not found.");

        switch (request.Status)
        {
            case BedStatus.Available:
                bed.MarkAvailable();
                break;

            case BedStatus.Maintenance:
                bed.MarkForMaintenance();
                break;

            case BedStatus.Inactive:
                bed.Deactivate();
                break;

            default:
                return BadRequest("Invalid bed status.");
        }

        await _context.SaveChangesAsync();

        return Ok("Bed status updated successfully.");
    }

    // DELETE: api/Bed/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBed(Guid id)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return NotFound("Bed not found.");

        _context.Beds.Remove(bed);
        await _context.SaveChangesAsync();

        return Ok("Bed deleted successfully.");
    }
}

public class BedRequest
{
    public Guid RoomId { get; set; }
    public string BedNumber { get; set; } = string.Empty;
}

public class BedUpdateRequest
{
    public string BedNumber { get; set; } = string.Empty;
}

public class BedStatusRequest
{
    public BedStatus Status { get; set; }
}
