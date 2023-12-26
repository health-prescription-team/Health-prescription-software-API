using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class AddFulfilledByToPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "FulfilledById",
                table: "Prescriptions",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19a61a34-ba06-400e-8bb3-e8b3cd364640", null, "Pharmacist", "PHARMACIST" },
                    { "b6b13566-a0fc-460c-be05-35b2ce5700d1", null, "GP", "GP" },
                    { "d88b8f8c-6b56-439a-b101-7dc48eb2b994", null, "Pharmacy", "PHARMACY" },
                    { "eadfaa84-8388-4f6f-a647-9c2429f17bb6", null, "Patient", "PATIENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_FulfilledById",
                table: "Prescriptions",
                column: "FulfilledById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_GpId",
                table: "Prescriptions",
                column: "GpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_AspNetUsers_FulfilledById",
                table: "Prescriptions",
                column: "FulfilledById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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
                name: "FK_Prescriptions_AspNetUsers_FulfilledById",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_AspNetUsers_GpId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_FulfilledById",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_GpId",
                table: "Prescriptions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19a61a34-ba06-400e-8bb3-e8b3cd364640");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6b13566-a0fc-460c-be05-35b2ce5700d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d88b8f8c-6b56-439a-b101-7dc48eb2b994");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eadfaa84-8388-4f6f-a647-9c2429f17bb6");

            migrationBuilder.DropColumn(
                name: "FulfilledById",
                table: "Prescriptions");

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
        }
    }
}
