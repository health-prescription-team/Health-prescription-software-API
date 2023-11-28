using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class added_prescriptionDetails_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4df7e291-6a54-440d-9f5a-e85747e82dd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69926e5a-f5e6-4d07-8975-d280b4c04734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b8c95584-1c2c-4264-8e1a-8593d21adf26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f404b77f-0eae-42cb-a1f8-3110cfed0968");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Prescriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedAt",
                table: "Prescriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PrescriptionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: true),
                    EveningDose = table.Column<int>(type: "int", nullable: false),
                    LunchTimeDose = table.Column<int>(type: "int", nullable: false),
                    MorningDose = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetails_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4285cf72-f2d2-4f17-be72-c6519c8c1d41", null, "Patient", "PATIENT" },
                    { "730bdc3d-8125-4183-89bb-db9796988958", null, "GP", "GP" },
                    { "83c6dca0-9359-42da-943b-29fe00170357", null, "Pharmacist", "PHARMACIST" },
                    { "96ece562-f103-4c28-a4db-0e902e3eb1c6", null, "Pharmacy", "PHARMACY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_PrescriptionId",
                table: "PrescriptionDetails",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionDetails");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4285cf72-f2d2-4f17-be72-c6519c8c1d41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "730bdc3d-8125-4183-89bb-db9796988958");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83c6dca0-9359-42da-943b-29fe00170357");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96ece562-f103-4c28-a4db-0e902e3eb1c6");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "Prescriptions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4df7e291-6a54-440d-9f5a-e85747e82dd4", null, "Pharmacy", "PHARMACY" },
                    { "69926e5a-f5e6-4d07-8975-d280b4c04734", null, "Patient", "PATIENT" },
                    { "b8c95584-1c2c-4264-8e1a-8593d21adf26", null, "Pharmacist", "PHARMACIST" },
                    { "f404b77f-0eae-42cb-a1f8-3110cfed0968", null, "GP", "GP" }
                });
        }
    }
}
