using System;

namespace ProRandevu.Data.Models
{
    /// <summary>
    /// Appointment record for a service.
    /// </summary>
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int StaffId { get; set; }
        public Staff? Staff { get; set; }
    }

    /// <summary>
    /// Status of the appointment.
    /// </summary>
    public enum AppointmentStatus
    {
        Pending,
        Arrived,
        Cancelled
    }
}
