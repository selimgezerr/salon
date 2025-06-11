using System.Collections.Generic;
using System;
using System.Linq;
using ProRandevu.Data.Models;

namespace ProRandevu.Data.Data;

/// <summary>
/// Seeds the database with demo data if empty.
/// </summary>
public static class DemoVeriSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Skip seeding if any data already exists
        if (context.Customers.Any())
            return;

        var today = DateTime.Today;
        var customers = new List<Customer>
        {
            new() { FirstName = "Ali", LastName = "Yilmaz", Phone = "5551112233", CreatedAt = today.AddMonths(-2) },
            new() { FirstName = "Ayse", LastName = "Demir", Phone = "5553334455", CreatedAt = today.AddMonths(-1) },
            new() { FirstName = "Mehmet", LastName = "Kaya", Phone = "5557778899", CreatedAt = today },
        };
        context.Customers.AddRange(customers);

        var staffList = new List<Staff>
        {
            new() { Name = "Kemal", Expertise = "Berber", IsActive = true },
            new() { Name = "Fatma", Expertise = "Kuafor", IsActive = true }
        };
        context.Staff.AddRange(staffList);

        var services = new List<Service>
        {
            new() { Name = "Sac Kesimi", DurationMinutes = 30, Price = 50 },
            new() { Name = "Sakal Tiraşi", DurationMinutes = 15, Price = 30 },
            new() { Name = "Boyama", DurationMinutes = 60, Price = 150 },
            new() { Name = "Fön", DurationMinutes = 20, Price = 40 }
        };
        context.Services.AddRange(services);

        var appointments = new List<Appointment>
        {
            new()
            {
                Customer = customers[0],
                Service = services[0],
                Staff = staffList[0],
                StartTime = DateTime.Today.AddHours(10),
                EndTime = DateTime.Today.AddHours(10.5),
                Status = AppointmentStatus.Arrived
            },
            new()
            {
                Customer = customers[1],
                Service = services[2],
                Staff = staffList[1],
                StartTime = DateTime.Today.AddHours(12),
                EndTime = DateTime.Today.AddHours(13),
                Status = AppointmentStatus.Pending
            },
            new()
            {
                Customer = customers[2],
                Service = services[1],
                Staff = staffList[0],
                StartTime = DateTime.Today.AddHours(14),
                EndTime = DateTime.Today.AddHours(14.25),
                Status = AppointmentStatus.Cancelled
            }
        };
        context.Appointments.AddRange(appointments);

        var payments = new List<Payment>
        {
            new() { Customer = customers[0], Date = DateTime.Today, Amount = 50, Type = PaymentType.Cash, Note = "Haircut" },
            new() { Customer = customers[1], Date = DateTime.Today, Amount = 150, Type = PaymentType.Card, Note = "Coloring" },
            new() { Customer = customers[2], Date = DateTime.Today, Amount = 30, Type = PaymentType.Debt, Note = "Shave" }
        };
        context.Payments.AddRange(payments);

        // Default settings row
        context.Settings.Add(new Setting
        {
            Theme = "Light",
            BackupPath = null,
            DefaultAppointmentDuration = 30,
            LicenseKey = null
        });

        context.SaveChanges();
    }
}
