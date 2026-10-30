
namespace HostelManagement.Application.DTOs.Resident;

public class UpdateResidentDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly AdmissionDate { get; set; }
}
