using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Migrations
{
    public partial class AddRentalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Returned",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                columns: new[] { "DateOfBirth", "DriversLicenseDate" },
                values: new object[] { new DateTime(2000, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c72d2e73-93b6-4ddb-bf6e-c778dd425e6b"),
                columns: new[] { "DateOfBirth", "DriversLicenseDate" },
                values: new object[] { new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2008, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Returned",
                table: "Rentals");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bbc591e4-eb41-4f8d-a030-1e892393224a"),
                columns: new[] { "DateOfBirth", "DriversLicenseDate" },
                values: new object[] { new DateTime(2021, 12, 23, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c72d2e73-93b6-4ddb-bf6e-c778dd425e6b"),
                columns: new[] { "DateOfBirth", "DriversLicenseDate" },
                values: new object[] { new DateTime(2021, 12, 23, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
