
using HostelManagement.Domain.Enums;

namespace HostelManagement.Application.DTOs.Room;

public class CreateRoomDto
{
    public Guid HostelId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
}
