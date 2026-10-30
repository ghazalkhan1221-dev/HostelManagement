
using HostelManagement.Application.DTOs.Room;
using HostelManagement.Application.Interfaces.Services;
using HostelManagement.Domain.Entities;
using HostelManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.Infrastructure.Services;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;

    public RoomService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<IEnumerable<RoomResponseDto>> GetAllAsync()
    {
        var rooms = await _context.Rooms.ToListAsync();

        return rooms.Select(MapToDto).ToList();
    }

    // GET BY ID
    public async Task<RoomResponseDto?> GetByIdAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        return room == null ? null : MapToDto(room);
    }

    // CREATE
    public async Task<RoomResponseDto> CreateAsync(CreateRoomDto dto)
    {
        var hostelExists = await _context.Hostels
            .AnyAsync(h => h.Id == dto.HostelId);

        if (!hostelExists)
            throw new InvalidOperationException("Invalid HostelId.");

        var room = new Room(
            dto.HostelId,
            dto.RoomNumber,
            dto.FloorNumber,
            dto.Capacity,
            dto.Type);

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync();

        return MapToDto(room);
    }

    // UPDATE
    public async Task<bool> UpdateAsync(
        Guid id, UpdateRoomDto dto)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return false;

        room.UpdateDetails(
            dto.RoomNumber,
            dto.FloorNumber,
            dto.Capacity,
            dto.Type);

        await _context.SaveChangesAsync();

        return true;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return false;

        var hasBeds = await _context.Beds
            .AnyAsync(b => b.RoomId == id);

        if (hasBeds)
            throw new InvalidOperationException(
                "Room has beds and cannot be deleted.");

        _context.Rooms.Remove(room);

        await _context.SaveChangesAsync();

        return true;
    }

    // ENTITY TO DTO
    private static RoomResponseDto MapToDto(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            HostelId = room.HostelId,
            RoomNumber = room.RoomNumber,
            FloorNumber = room.FloorNumber,
            Capacity = room.Capacity,
            Type = room.Type
        };
    }
}
