using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class InvitesUpdateReferee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefereeInvited",
                table: "Matches",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Matches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchInvitedToId",
                table: "Invites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RefereeInvite",
                table: "Invites",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Adress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LocationId",
                table: "Matches",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_MatchInvitedToId",
                table: "Invites",
                column: "MatchInvitedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_Matches_MatchInvitedToId",
                table: "Invites",
                column: "MatchInvitedToId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Locations_LocationId",
                table: "Matches",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_Matches_MatchInvitedToId",
                table: "Invites");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Locations_LocationId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Matches_LocationId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Invites_MatchInvitedToId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "IsRefereeInvited",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchInvitedToId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "RefereeInvite",
                table: "Invites");
        }
    }
}
