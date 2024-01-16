using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class CreatedChatEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ReceiverId = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "621f862e-124e-400e-91e8-4cb50989becb", null, "GP", "GP" },
                    { "a30e0654-4261-4d7d-9a6d-d08e2942d8a3", null, "Pharmacist", "PHARMACIST" },
                    { "e04443d5-2e00-4c86-a0d8-26ac80cd52d7", null, "Patient", "PATIENT" },
                    { "ebe99957-591c-4d4a-9226-950491378ceb", null, "Pharmacy", "PHARMACY" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "621f862e-124e-400e-91e8-4cb50989becb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a30e0654-4261-4d7d-9a6d-d08e2942d8a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e04443d5-2e00-4c86-a0d8-26ac80cd52d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebe99957-591c-4d4a-9226-950491378ceb");

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
        }
    }
}
