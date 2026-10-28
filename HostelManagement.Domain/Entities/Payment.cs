using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid FeeChargeId { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly PaymentDate { get; private set; }

    public string PaymentMethod { get; private set; } = string.Empty;

    public string? ReferenceNumber { get; private set; }

    public FeeCharge? FeeCharge { get; private set; }

    private Payment()
    {
        // Entity Framework Core ke liye
    }

    public Payment(
        Guid feeChargeId,
        decimal amount,
        DateOnly paymentDate,
        string paymentMethod,
        string? referenceNumber = null)
    {
        if (feeChargeId == Guid.Empty)
            throw new ArgumentException(
                "Fee charge is required.", nameof(feeChargeId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Payment must be greater than zero.");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new ArgumentException(
                "Payment method is required.", nameof(paymentMethod));

        FeeChargeId = feeChargeId;
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod.Trim();
        ReferenceNumber = string.IsNullOrWhiteSpace(referenceNumber)
            ? null
            : referenceNumber.Trim();
    }
}
