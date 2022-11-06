using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.API.Migrations
{
    public partial class AdditionOfRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ea9549d-7e84-480b-8489-3085ee2871f6", "ddd26da7-2041-46ec-a263-f235d714962f", "User", "USER" },
                    { "903ad525-a242-4ea0-9b2e-2f84709ff13b", "8ca6813d-ef51-42eb-8477-cd966c110d67", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ea9549d-7e84-480b-8489-3085ee2871f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "903ad525-a242-4ea0-9b2e-2f84709ff13b");
        }
    }
}
