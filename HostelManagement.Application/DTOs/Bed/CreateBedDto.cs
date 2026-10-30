namespace HostelManagement.Application.DTOs.Bed;

public class CreateBedDto
{
    public Guid RoomId { get; set; }
    public string BedNumber { get; set; } = string.Empty;
}