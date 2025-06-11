namespace ProRandevu.Data.Models
{
    /// <summary>
    /// Staff working in the salon.
    /// </summary>
    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Expertise { get; set; }
        /// <summary>
        /// Indicates whether the staff member is currently active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
