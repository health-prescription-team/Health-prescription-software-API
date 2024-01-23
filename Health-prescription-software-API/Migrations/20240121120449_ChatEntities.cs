using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_prescription_software_API.Migrations
{
    /// <inheritdoc />
    public partial class ChatEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    UserOneId = table.Column<string>(type: "text", nullable: false),
                    UserTwoId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => new { x.UserOneId, x.UserTwoId });
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MessageTime = table.Column<DateTime>(type: "Timestamp", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ConversationUserOneId = table.Column<string>(type: "text", nullable: true),
                    ConversationUserTwoId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationUserOneId_ConversationUs~",
                        columns: x => new { x.ConversationUserOneId, x.ConversationUserTwoId },
                        principalTable: "Conversations",
                        principalColumns: new[] { "UserOneId", "UserTwoId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationUserOneId_ConversationUserTwoId",
                table: "Messages",
                columns: new[] { "ConversationUserOneId", "ConversationUserTwoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");
        }
    }
}
