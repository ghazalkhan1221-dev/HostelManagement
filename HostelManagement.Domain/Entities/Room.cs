using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class Room
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid HostelId { get; private set; }

        public string RoomNumber { get; private set; } = string.Empty;

        public int FloorNumber { get; private set; }

        public int Capacity { get; private set; }

        public RoomType Type { get; private set; }

        public Hostel? Hostel { get; private set; }

        public ICollection<Bed> Beds { get; private set; } = new List<Bed>();

        private Room()
        {
            // Entity Framework Core ke liye
        }

        public Room(
            Guid hostelId,
            string roomNumber,
            int floorNumber,
            int capacity,
            RoomType type)
        {
            if (hostelId == Guid.Empty)
                throw new ArgumentException("Hostel is required.", nameof(hostelId));

            if (string.IsNullOrWhiteSpace(roomNumber))
                throw new ArgumentException("Room number is required.", nameof(roomNumber));

            if (floorNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(floorNumber));

            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            HostelId = hostelId;
            RoomNumber = roomNumber.Trim();
            FloorNumber = floorNumber;
            Capacity = capacity;
            Type = type;
        }
    }
