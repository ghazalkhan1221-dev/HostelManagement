namespace HostelManagement.Application.DTOs.Hostel;

public class HostelResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}