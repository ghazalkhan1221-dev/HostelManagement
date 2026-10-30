
using HostelManagement.Application.DTOs.Bed;
using HostelManagement.Application.Interfaces.Services;
using HostelManagement.Domain.Entities;
using HostelManagement.Domain.Enums;
using HostelManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.Infrastructure.Services;

public class BedService : IBedService
{
    private readonly ApplicationDbContext _context;

    public BedService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<IEnumerable<BedResponseDto>> GetAllAsync()
    {
        var beds = await _context.Beds.ToListAsync();

        return beds.Select(MapToDto).ToList();
    }

    // GET BY ID
    public async Task<BedResponseDto?> GetByIdAsync(Guid id)
    {
        var bed = await _context.Beds.FindAsync(id);

        return bed == null ? null : MapToDto(bed);
    }

    // CREATE
    public async Task<BedResponseDto> CreateAsync(CreateBedDto dto)
    {
        var roomExists = await _context.Rooms
            .AnyAsync(r => r.Id == dto.RoomId);

        if (!roomExists)
            throw new InvalidOperationException("Invalid RoomId.");

        var bed = new Bed(dto.RoomId, dto.BedNumber);

        _context.Beds.Add(bed);

        await _context.SaveChangesAsync();

        return MapToDto(bed);
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Guid id, UpdateBedDto dto)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return false;

        bed.UpdateBedNumber(dto.BedNumber);

        await _context.SaveChangesAsync();

        return true;
    }

    // UPDATE STATUS
    public async Task<bool> UpdateStatusAsync(
        Guid id, BedStatus status)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return false;

        switch (status)
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
                throw new ArgumentException("Invalid Bed Status.");
        }

        await _context.SaveChangesAsync();

        return true;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var bed = await _context.Beds.FindAsync(id);

        if (bed == null)
            return false;

        var isAllocated = await _context.ResidentAllocations
            .AnyAsync(a => a.BedId == id);

        if (isAllocated)
            throw new InvalidOperationException(
                "Bed has allocation records and cannot be deleted.");

        _context.Beds.Remove(bed);

        await _context.SaveChangesAsync();

        return true;
    }

    // ENTITY TO DTO
    private static BedResponseDto MapToDto(Bed bed)
    {
        return new BedResponseDto
        {
            Id = bed.Id,
            RoomId = bed.RoomId,
            BedNumber = bed.BedNumber,
            Status = bed.Status
        };
    }
}
    