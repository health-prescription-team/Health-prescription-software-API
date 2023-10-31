using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class seeded_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HostpitalName",
                table: "AspNetUsers",
                newName: "HospitalName");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "027f66e9-83ed-4902-b1d7-53103ff49295", null, "Pharmacist", "PHARMACIST" },
                    { "6340aba9-073e-4117-a230-8cab740efd20", null, "Patient", "PATIENT" },
                    { "7072dfd3-a3d8-4bf6-ad08-e42e881aee02", null, "Pharmacy", "PHARMACY" },
                    { "d4a622a5-0a7a-421a-a8ee-bb52305eb298", null, "GP", "GP" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "027f66e9-83ed-4902-b1d7-53103ff49295");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6340aba9-073e-4117-a230-8cab740efd20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7072dfd3-a3d8-4bf6-ad08-e42e881aee02");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4a622a5-0a7a-421a-a8ee-bb52305eb298");

            migrationBuilder.RenameColumn(
                name: "HospitalName",
                table: "AspNetUsers",
                newName: "HostpitalName");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
