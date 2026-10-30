
using HostelManagement.Application.DTOs.Resident;

namespace HostelManagement.Application.Interfaces.Services;

public interface IResidentService
{
    Task<IEnumerable<ResidentResponseDto>> GetAllAsync();

    Task<ResidentResponseDto?> GetByIdAsync(Guid id);

    Task<ResidentResponseDto> CreateAsync(CreateResidentDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateResidentDto dto);

    Task<bool> DeleteAsync(Guid id);

    Task<bool> CheckoutAsync(Guid id);

    Task<bool> MarkInactiveAsync(Guid id);
}
