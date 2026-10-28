using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class ResidentAllocation
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid ResidentId { get; private set; }

        public Guid BedId { get; private set; }

        public DateOnly StartDate { get; private set; }

        public DateOnly? EndDate { get; private set; }

        public AllocationStatus Status { get; private set; }
            = AllocationStatus.Active;

        public Resident? Resident { get; private set; }

        public Bed? Bed { get; private set; }

        private ResidentAllocation()
        {
            // Entity Framework Core ke liye
        }

        public ResidentAllocation(
            Guid residentId,
            Guid bedId,
            DateOnly startDate)
        {
            if (residentId == Guid.Empty)
                throw new ArgumentException("Resident is required.", nameof(residentId));

            if (bedId == Guid.Empty)
                throw new ArgumentException("Bed is required.", nameof(bedId));

            ResidentId = residentId;
            BedId = bedId;
            StartDate = startDate;
            EndDate = null;
            Status = AllocationStatus.Active;
        }

        public void Checkout(DateOnly checkoutDate)
        {
            if (Status != AllocationStatus.Active)
                throw new InvalidOperationException(
                    "Only an active allocation can be checked out.");

            if (checkoutDate < StartDate)
                throw new ArgumentException(
                    "Checkout date cannot be before the start date.",
                    nameof(checkoutDate));

            EndDate = checkoutDate;
            Status = AllocationStatus.CheckedOut;
        }

        public void Cancel()
        {
            if (Status != AllocationStatus.Active)
                throw new InvalidOperationException(
                    "Only an active allocation can be cancelled.");

            Status = AllocationStatus.Cancelled;
            EndDate = StartDate;
        }
    }

