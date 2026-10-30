using HostelManagement.Application.DTOs.Bed;
using HostelManagement.Domain.Enums;

namespace HostelManagement.Application.Interfaces.Services;

public interface IBedService
{
    Task<IEnumerable<BedResponseDto>> GetAllAsync();

    Task<BedResponseDto?> GetByIdAsync(Guid id);

    Task<BedResponseDto> CreateAsync(CreateBedDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateBedDto dto);

    Task<bool> UpdateStatusAsync(Guid id, BedStatus status);

    Task<bool> DeleteAsync(Guid id);
}