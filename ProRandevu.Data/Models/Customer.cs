using System;

namespace ProRandevu.Data.Models
{

/// <summary>
/// Customer entity.
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Note { get; set; }
    /// <summary>
    /// Optional path of the customer's photo on disk.
    /// </summary>
    public string? PhotoPath { get; set; }
    /// <summary>
    /// Date when the customer record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int LoyaltyPoints { get; set; }
}
}
