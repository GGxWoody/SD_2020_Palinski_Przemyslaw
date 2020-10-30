using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class StartOnInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdFrom = table.Column<int>(type: "INTEGER", nullable: false),
                    InviteFromId = table.Column<int>(type: "INTEGER", nullable: true),
                    IdTo = table.Column<int>(type: "INTEGER", nullable: false),
                    InviteToId = table.Column<int>(type: "INTEGER", nullable: true),
                    FriendInvite = table.Column<bool>(type: "INTEGER", nullable: false),
                    TeamInvite = table.Column<bool>(type: "INTEGER", nullable: false),
                    MatchInvite = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invites_Users_InviteFromId",
                        column: x => x.InviteFromId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invites_Users_InviteToId",
                        column: x => x.InviteToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InviteFromId",
                table: "Invites",
                column: "InviteFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InviteToId",
                table: "Invites",
                column: "InviteToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");
        }
    }
}
