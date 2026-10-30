using HostelManagement.Domain.Enums;

namespace HostelManagement.Application.DTOs.Bed;

public class BedResponseDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public string BedNumber { get; set; } = string.Empty;
    public BedStatus Status { get; set; }
}