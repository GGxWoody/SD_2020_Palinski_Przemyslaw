using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class AddedTeamToInvites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamInvitedId",
                table: "Invites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_TeamInvitedId",
                table: "Invites",
                column: "TeamInvitedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Teams_TeamInvitedId",
                table: "Invites",
                column: "TeamInvitedId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Teams_TeamInvitedId",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_TeamInvitedId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "TeamInvitedId",
                table: "Invites");
        }
    }
}
