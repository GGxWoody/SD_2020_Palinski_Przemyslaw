using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class FixOnInvite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFrom",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "IdTo",
                table: "Invites");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdFrom",
                table: "Invites",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTo",
                table: "Invites",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
