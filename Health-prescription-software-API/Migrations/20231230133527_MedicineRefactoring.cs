using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class MedicineRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersMedicines");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b20d36b-7fba-4efa-9afb-8a7fa7272d21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7096ca2c-d8ce-413b-9d96-a87112825452");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be85a352-0017-4d97-8b65-3ec0e40d188c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db016c26-261e-4bc5-944f-04a06470b425");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "AveragePrice",
                table: "Medicines",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "Medicines",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Medicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1789daa2-03da-4927-bdd2-b26158614200", null, "GP", "GP" },
                    { "4a7e5619-8a50-4030-9d73-ece39921e0bb", null, "Pharmacy", "PHARMACY" },
                    { "a2093fdd-44ad-466d-99dd-66d0e6dfab37", null, "Patient", "PATIENT" },
                    { "fa8d5cb4-713a-4ee2-9a61-fa316e8189a2", null, "Pharmacist", "PHARMACIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_GpId",
                table: "Prescriptions",
                column: "GpId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_OwnerId",
                table: "Medicines",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_OwnerId",
                table: "Medicines",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_AspNetUsers_GpId",
                table: "Prescriptions",
                column: "GpId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_OwnerId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_AspNetUsers_GpId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_GpId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_OwnerId",
                table: "Medicines");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1789daa2-03da-4927-bdd2-b26158614200");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a7e5619-8a50-4030-9d73-ece39921e0bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2093fdd-44ad-466d-99dd-66d0e6dfab37");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa8d5cb4-713a-4ee2-9a61-fa316e8189a2");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Medicines");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Medicines",
                newName: "AveragePrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medicines",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UsersMedicines",
                columns: table => new
                {
                    MedicineId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    MedicinePrice = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersMedicines", x => new { x.MedicineId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersMedicines_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b20d36b-7fba-4efa-9afb-8a7fa7272d21", null, "Patient", "PATIENT" },
                    { "7096ca2c-d8ce-413b-9d96-a87112825452", null, "Pharmacy", "PHARMACY" },
                    { "be85a352-0017-4d97-8b65-3ec0e40d188c", null, "Pharmacist", "PHARMACIST" },
                    { "db016c26-261e-4bc5-944f-04a06470b425", null, "GP", "GP" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersMedicines_UserId",
                table: "UsersMedicines",
                column: "UserId");
        }
    }
}
