using HostelManagement.Domain.Enums;

namespace HostelManagement.Domain.Entities;

public class Resident
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string FullName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string PhoneNumber { get; private set; } = string.Empty;

        public DateOnly AdmissionDate { get; private set; }

        public ResidentStatus Status { get; private set; } = ResidentStatus.Active;

        public ICollection<ResidentAllocation> Allocations { get; private set; }
            = new List<ResidentAllocation>();

        private Resident()
        {
            // Entity Framework Core ke liye
        }

        public Resident(
            string fullName,
            string email,
            string phoneNumber,
            DateOnly admissionDate)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Resident name is required.", nameof(fullName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Resident email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

            FullName = fullName.Trim();
            Email = email.Trim();
            PhoneNumber = phoneNumber.Trim();
            AdmissionDate = admissionDate;
            Status = ResidentStatus.Active;
        }

        public void MarkCheckedOut()
        {
            Status = ResidentStatus.CheckedOut;
        }

        public void MarkInactive()
        {
            Status = ResidentStatus.Inactive;
        }
    public void UpdateDetails(
         string fullName,
          string email,
          string phoneNumber,
          DateOnly admissionDate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Resident name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Resident email is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.");

        FullName = fullName.Trim();
        Email = email.Trim();
        PhoneNumber = phoneNumber.Trim();
        AdmissionDate = admissionDate;
    }


}


