using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Migrations
{
    public partial class AddCarTimeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeAdded",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "Description", "Horsepower", "Model", "TimeAdded", "YearOfProduction" },
                values: new object[] { 1, "Lego", "Taki fajny kolorowy i szybki", 9999, "Custom Model", new DateTime(2021, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2040 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthID", "DateOfBirth", "DriversLicenseDate", "Email", "Location", "Role" },
                values: new object[] { 1, "googleid2", new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2@website.com", "Warsaw", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthID", "DateOfBirth", "DriversLicenseDate", "Email", "Location", "Role" },
                values: new object[] { 2, "googleid", new DateTime(2021, 12, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@website.com", "Warsaw", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "Cars");
        }
    }
}
