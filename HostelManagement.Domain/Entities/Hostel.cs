using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class Hostel
        {
            public Guid Id { get; private set; } = Guid.NewGuid();

            public string Name { get; private set; } = string.Empty;

            public string Address { get; private set; } = string.Empty;

            public ICollection<Room> Rooms { get; private set; }
                = new List<Room>();

            private Hostel()
            {
                // Entity Framework Core ke liye
            }

            public Hostel(string name, string address)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Hostel name is required.", nameof(name));

                if (string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("Hostel address is required.", nameof(address));

                Name = name.Trim();
                Address = address.Trim();
            }
        }
    