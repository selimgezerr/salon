using Microsoft.EntityFrameworkCore;
using ProRandevu.Data.Data;

// Setup DbContext with SQLite connection
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=pro.db")
    .Options;

using var context = new AppDbContext(options);

// Apply migrations and seed demo data
context.Database.Migrate();
DemoVeriSeeder.Seed(context);

Console.WriteLine("Database created and seeded");
