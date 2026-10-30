
using HostelManagement.Application.DTOs.Room;

namespace HostelManagement.Application.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomResponseDto>> GetAllAsync();

    Task<RoomResponseDto?> GetByIdAsync(Guid id);

    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateRoomDto dto);

    Task<bool> DeleteAsync(Guid id);
}
