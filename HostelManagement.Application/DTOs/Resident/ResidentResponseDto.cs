
using HostelManagement.Domain.Enums;

namespace HostelManagement.Application.DTOs.Resident;

public class ResidentResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly AdmissionDate { get; set; }
    public ResidentStatus Status { get; set; }
}
