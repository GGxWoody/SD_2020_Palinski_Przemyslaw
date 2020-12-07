using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class MatchRefereeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefereeId",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RefereeId",
                table: "Matches",
                column: "RefereeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_RefereeId",
                table: "Matches",
                column: "RefereeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_RefereeId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_RefereeId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "RefereeId",
                table: "Matches");
        }
    }
}
