using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class FeeCharge
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid ResidentId { get; private set; }

    public int BillingYear { get; private set; }

    public int BillingMonth { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly DueDate { get; private set; }

    public Resident? Resident { get; private set; }

    public ICollection<Payment> Payments { get; private set; }
        = new List<Payment>();

    private FeeCharge()
    {
        // Entity Framework Core ke liye
    }

    public FeeCharge(
        Guid residentId,
        int billingYear,
        int billingMonth,
        decimal amount,
        DateOnly dueDate)
    {
        if (residentId == Guid.Empty)
            throw new ArgumentException("Resident is required.");

        if (billingYear < 2000 || billingYear > 9999)
            throw new ArgumentOutOfRangeException(nameof(billingYear));

        if (billingMonth < 1 || billingMonth > 12)
            throw new ArgumentOutOfRangeException(nameof(billingMonth));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Fee must be greater than zero.");

        ResidentId = residentId;
        BillingYear = billingYear;
        BillingMonth = billingMonth;
        Amount = amount;
        DueDate = dueDate;
    }
}
