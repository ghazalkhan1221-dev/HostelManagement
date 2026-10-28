using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class Complaint
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ResidentId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string Status { get; private set; } = "Open";

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; private set; }

    public Resident? Resident { get; private set; }

    private Complaint()
    {
        // Entity Framework Core ke liye
    }

    public Complaint(
        Guid residentId,
        string title,
        string description)
    {
        if (residentId == Guid.Empty)
            throw new ArgumentException(
                "Resident is required.", nameof(residentId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Complaint title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Complaint description is required.", nameof(description));

        ResidentId = residentId;
        Title = title.Trim();
        Description = description.Trim();
        Status = "Open";
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkInProgress()
    {
        if (Status != "Open")
            throw new InvalidOperationException(
                "Only an open complaint can be moved to in progress.");

        Status = "InProgress";
    }

    public void Resolve()
    {
        if (Status == "Resolved")
            throw new InvalidOperationException(
                "Complaint is already resolved.");

        Status = "Resolved";
        ResolvedAt = DateTime.UtcNow;
    }
}
