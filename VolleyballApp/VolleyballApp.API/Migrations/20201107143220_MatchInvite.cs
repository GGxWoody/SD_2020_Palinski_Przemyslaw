using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class MatchInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamInvitingId",
                table: "Invites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_TeamInvitingId",
                table: "Invites",
                column: "TeamInvitingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Teams_TeamInvitingId",
                table: "Invites",
                column: "TeamInvitingId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Teams_TeamInvitingId",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_TeamInvitingId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "TeamInvitingId",
                table: "Invites");
        }
    }
}
