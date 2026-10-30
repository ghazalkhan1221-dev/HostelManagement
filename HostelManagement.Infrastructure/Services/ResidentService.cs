
using HostelManagement.Application.DTOs.Resident;
using HostelManagement.Application.Interfaces.Services;
using HostelManagement.Domain.Entities;
using HostelManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HostelManagement.Infrastructure.Services;

public class ResidentService : IResidentService
{
    private readonly ApplicationDbContext _context;

    public ResidentService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<IEnumerable<ResidentResponseDto>> GetAllAsync()
    {
        var residents = await _context.Residents.ToListAsync();

        return residents.Select(MapToDto).ToList();
    }

    // GET BY ID
    public async Task<ResidentResponseDto?> GetByIdAsync(Guid id)
    {
        var resident = await _context.Residents.FindAsync(id);

        return resident == null ? null : MapToDto(resident);
    }

    // CREATE
    public async Task<ResidentResponseDto> CreateAsync(
        CreateResidentDto dto)
    {
        var resident = new Resident(
            dto.FullName,
            dto.Email,
            dto.PhoneNumber,
            dto.AdmissionDate);

        _context.Residents.Add(resident);

        await _context.SaveChangesAsync();

        return MapToDto(resident);
    }

    // UPDATE
    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateResidentDto dto)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
            return false;

        resident.UpdateDetails(
            dto.FullName,
            dto.Email,
            dto.PhoneNumber,
            dto.AdmissionDate);

        await _context.SaveChangesAsync();

        return true;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
            return false;

        var hasAllocations = await _context.ResidentAllocations
            .AnyAsync(a => a.ResidentId == id);

        if (hasAllocations)
            throw new InvalidOperationException(
                "Resident has allocation records and cannot be deleted.");

        _context.Residents.Remove(resident);

        await _context.SaveChangesAsync();

        return true;
    }

    // CHECKOUT
    public async Task<bool> CheckoutAsync(Guid id)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
            return false;

        resident.MarkCheckedOut();

        await _context.SaveChangesAsync();

        return true;
    }

    // MARK INACTIVE
    public async Task<bool> MarkInactiveAsync(Guid id)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
            return false;

        resident.MarkInactive();

        await _context.SaveChangesAsync();

        return true;
    }

    // ENTITY TO DTO MAPPING
    private static ResidentResponseDto MapToDto(Resident resident)
    {
        return new ResidentResponseDto
        {
            Id = resident.Id,
            FullName = resident.FullName,
            Email = resident.Email,
            PhoneNumber = resident.PhoneNumber,
            AdmissionDate = resident.AdmissionDate,
            Status = resident.Status
        };
    }
}
