
using HostelManagement.Domain.Entities;
using HostelManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Room
    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _context.Rooms
            .Select(r => new
            {
                r.Id,
                r.HostelId,
                r.RoomNumber,
                r.FloorNumber,
                r.Capacity,
                r.Type
            })
            .ToListAsync();

        return Ok(rooms);
    }

    // GET: api/Room/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        var room = await _context.Rooms
            .Where(r => r.Id == id)
            .Select(r => new
            {
                r.Id,
                r.HostelId,
                r.RoomNumber,
                r.FloorNumber,
                r.Capacity,
                r.Type
            })
            .FirstOrDefaultAsync();

        if (room == null)
            return NotFound("Room not found.");

        return Ok(room);
    }

    // POST: api/Room
    [HttpPost]
    public async Task<IActionResult> CreateRoom(
        [FromBody] RoomRequest request)
    {
        var hostelExists = await _context.Hostels
            .AnyAsync(h => h.Id == request.HostelId);

        if (!hostelExists)
            return BadRequest("Invalid HostelId.");

        var room = new Room(
            request.HostelId,
            request.RoomNumber,
            request.FloorNumber,
            request.Capacity,
            request.Type);

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetRoomById),
            new { id = room.Id },
            new
            {
                room.Id,
                room.HostelId,
                room.RoomNumber,
                room.FloorNumber,
                room.Capacity,
                room.Type
            });
    }

    // PUT: api/Room/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(
        Guid id,
        [FromBody] RoomUpdateRequest request)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound("Room not found.");

        room.UpdateDetails(
            request.RoomNumber,
            request.FloorNumber,
            request.Capacity,
            request.Type);

        await _context.SaveChangesAsync();

        return Ok("Room updated successfully.");
    }

    // DELETE: api/Room/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound("Room not found.");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return Ok("Room deleted successfully.");
    }
}

public class RoomRequest
{
    public Guid HostelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public int Capacity { get; set; }
    public HostelManagement.Domain.Enums.RoomType Type { get; set; }
}

public class RoomUpdateRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public int Capacity { get; set; }
    public HostelManagement.Domain.Enums.RoomType Type { get; set; }
}
