using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KerryCoAdmin.Api.Migrations
{
    public partial class productUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Admins_AdminId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariation_Product_ProductId",
                table: "ProductVariation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariation",
                table: "ProductVariation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c5fb515-28f5-482e-a837-62f29cf7d3c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c13ee270-a29e-499e-bafe-b79e4c71baf9");

            migrationBuilder.RenameTable(
                name: "ProductVariation",
                newName: "ProductVariations");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariation_ProductId",
                table: "ProductVariations",
                newName: "IX_ProductVariations_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_AdminId",
                table: "Products",
                newName: "IX_Products_AdminId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductVariations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "94040053-29c3-49b7-9cd5-16f272cf8f97", "1", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db2d0b37-bb42-4be8-bb0a-c02051355133", "2", "Staff", "Staff" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Admins_AdminId",
                table: "Products",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariations_Products_ProductId",
                table: "ProductVariations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Admins_AdminId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariations_Products_ProductId",
                table: "ProductVariations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariations",
                table: "ProductVariations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94040053-29c3-49b7-9cd5-16f272cf8f97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db2d0b37-bb42-4be8-bb0a-c02051355133");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductVariations");

            migrationBuilder.RenameTable(
                name: "ProductVariations",
                newName: "ProductVariation");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariations_ProductId",
                table: "ProductVariation",
                newName: "IX_ProductVariation_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_AdminId",
                table: "Product",
                newName: "IX_Product_AdminId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariation",
                table: "ProductVariation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9c5fb515-28f5-482e-a837-62f29cf7d3c1", "1", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c13ee270-a29e-499e-bafe-b79e4c71baf9", "2", "Staff", "Staff" });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Admins_AdminId",
                table: "Product",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariation_Product_ProductId",
                table: "ProductVariation",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");
        }
    }
}
