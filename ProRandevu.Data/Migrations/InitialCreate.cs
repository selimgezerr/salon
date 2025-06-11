using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProRandevu.Data.Migrations;

/// <summary>
/// Initial database schema for the salon application.
/// </summary>
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FirstName = table.Column<string>(nullable: false),
                LastName = table.Column<string>(nullable: false),
                Phone = table.Column<string>(nullable: false),
                BirthDate = table.Column<DateTime>(nullable: true),
                Note = table.Column<string>(nullable: true),
                PhotoPath = table.Column<string>(nullable: true),
                CreatedAt = table.Column<DateTime>(nullable: false),
                LoyaltyPoints = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Staff",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(nullable: false),
                Expertise = table.Column<string>(nullable: true),
                IsActive = table.Column<bool>(nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Staff", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Services",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(nullable: false),
                DurationMinutes = table.Column<int>(nullable: false),
                Price = table.Column<decimal>(nullable: false),
                Category = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Services", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Settings",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Theme = table.Column<string>(nullable: false),
                BackupPath = table.Column<string>(nullable: true),
                DefaultAppointmentDuration = table.Column<int>(nullable: false),
                LicenseKey = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Settings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Username = table.Column<string>(nullable: false),
                PasswordHash = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Appointments",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                StartTime = table.Column<DateTime>(nullable: false),
                EndTime = table.Column<DateTime>(nullable: false),
                Status = table.Column<int>(nullable: false),
                CustomerId = table.Column<int>(nullable: false),
                ServiceId = table.Column<int>(nullable: false),
                StaffId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Appointments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Appointments_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Appointments_Services_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "Services",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Appointments_Staff_StaffId",
                    column: x => x.StaffId,
                    principalTable: "Staff",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Date = table.Column<DateTime>(nullable: false),
                Amount = table.Column<decimal>(nullable: false),
                Type = table.Column<int>(nullable: false),
                Note = table.Column<string>(nullable: true),
                CustomerId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_CustomerId",
            table: "Appointments",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_ServiceId",
            table: "Appointments",
            column: "ServiceId");

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_StaffId",
            table: "Appointments",
            column: "StaffId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_CustomerId",
            table: "Payments",
            column: "CustomerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Appointments");
        migrationBuilder.DropTable(name: "Payments");
        migrationBuilder.DropTable(name: "Services");
        migrationBuilder.DropTable(name: "Staff");
        migrationBuilder.DropTable(name: "Customers");
        migrationBuilder.DropTable(name: "Settings");
        migrationBuilder.DropTable(name: "Users");
    }
}
