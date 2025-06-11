namespace ProRandevu.Data.Models
{
    /// <summary>
    /// Service provided by the salon.
    /// </summary>
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
    }
}
