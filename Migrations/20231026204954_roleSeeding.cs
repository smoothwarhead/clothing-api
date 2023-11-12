using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KerryCoAdmin.Api.Migrations
{
    public partial class roleSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d8c20911-49bf-4c33-8cc2-283f30d11a35", "1", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f0c8a4be-72f1-4571-8a7c-7ebe30af2ce8", "2", "Staff", "Staff" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8c20911-49bf-4c33-8cc2-283f30d11a35");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0c8a4be-72f1-4571-8a7c-7ebe30af2ce8");
        }
    }
}
