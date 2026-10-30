
using HostelManagement.Application.DTOs.Hostel;
using HostelManagement.Application.Interfaces.Services;
using HostelManagement.Infrastructure.Data;
using HostelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.Infrastructure.Services;

public class HostelService : IHostelService
{
    private readonly ApplicationDbContext _context;

    public HostelService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HostelResponseDto>> GetAllAsync()
    {
        return await _context.Hostels
            .Select(h => new HostelResponseDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address
            })
            .ToListAsync();
    }

    public async Task<HostelResponseDto?> GetByIdAsync(Guid id)
    {
        var hostel = await _context.Hostels
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hostel == null)
            return null;

        return new HostelResponseDto
        {
            Id = hostel.Id,
            Name = hostel.Name,
            Address = hostel.Address
        };
    }

    public async Task<HostelResponseDto> CreateAsync(CreateHostelDto dto)
    {
        var hostel = new Hostel(dto.Name, dto.Address);

        await _context.Hostels.AddAsync(hostel);
        await _context.SaveChangesAsync();

        return new HostelResponseDto
        {
            Id = hostel.Id,
            Name = hostel.Name,
            Address = hostel.Address
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateHostelDto dto)
    {
        var hostel = await _context.Hostels
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hostel == null)
            return false;

        hostel.UpdateDetails(dto.Name, dto.Address);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var hostel = await _context.Hostels
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hostel == null)
            return false;

        bool hasRooms = await _context.Rooms
            .AnyAsync(r => r.HostelId == id);

        if (hasRooms)
            throw new InvalidOperationException(
                "Cannot delete hostel because it has rooms.");

        _context.Hostels.Remove(hostel);

        await _context.SaveChangesAsync();

        return true;
    }
}
