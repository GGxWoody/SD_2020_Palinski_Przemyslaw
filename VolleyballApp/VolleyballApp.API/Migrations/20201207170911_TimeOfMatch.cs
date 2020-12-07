using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class TimeOfMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfMatch",
                table: "Matches",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfMatch",
                table: "Matches");
        }
    }
}
