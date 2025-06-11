namespace ProRandevu.Data.Models
{
    /// <summary>
    /// Application settings.
    /// </summary>
    public class Setting
    {
        public int Id { get; set; }

        /// <summary>
        /// Current application theme: "Light" or "Dark".
        /// </summary>
        public string Theme { get; set; } = "Light";

        /// <summary>
        /// Folder path where backups are stored.
        /// </summary>
        public string? BackupPath { get; set; }

        /// <summary>
        /// Default appointment duration in minutes.
        /// </summary>
        public int DefaultAppointmentDuration { get; set; } = 30;

        /// <summary>
        /// Optional license key string.
        /// </summary>
        public string? LicenseKey { get; set; }
    }
}
