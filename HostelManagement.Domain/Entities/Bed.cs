using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

    public class Bed
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid RoomId { get; private set; }

        public string BedNumber { get; private set; } = string.Empty;

        public BedStatus Status { get; private set; } = BedStatus.Available;

        public Room? Room { get; private set; }

        private Bed()
        {
            // Entity Framework Core ke liye
        }

        public Bed(Guid roomId, string bedNumber)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("Room is required.", nameof(roomId));

            if (string.IsNullOrWhiteSpace(bedNumber))
                throw new ArgumentException("Bed number is required.", nameof(bedNumber));

            RoomId = roomId;
            BedNumber = bedNumber.Trim();
            Status = BedStatus.Available;
        }

        public void MarkForMaintenance()
        {
            Status = BedStatus.Maintenance;
        }

        public void MarkAvailable()
        {
            Status = BedStatus.Available;
        }

        public void Deactivate()
        {
            Status = BedStatus.Inactive;
        }
    public void UpdateBedNumber(string bedNumber)
    {
        if (string.IsNullOrWhiteSpace(bedNumber))
            throw new ArgumentException("Bed number is required.");

        BedNumber = bedNumber.Trim();
    }
}

