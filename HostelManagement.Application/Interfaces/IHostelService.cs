using HostelManagement.Application.DTOs.Hostel;

namespace HostelManagement.Application.Interfaces.Services;

public interface IHostelService
{
    Task<IEnumerable<HostelResponseDto>> GetAllAsync();

    Task<HostelResponseDto?> GetByIdAsync(Guid id);

    Task<HostelResponseDto> CreateAsync(CreateHostelDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateHostelDto dto);

    Task<bool> DeleteAsync(Guid id);
}