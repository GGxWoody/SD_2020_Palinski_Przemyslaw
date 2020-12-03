using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class SetRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "FiveFirstTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FiveSecondTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourFirstTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourSecondTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneFirstTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneSecondTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeFirstTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeSecondTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoFirstTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoSecondTeam",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FiveFirstTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FiveSecondTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FourFirstTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FourSecondTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "OneFirstTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "OneSecondTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ThreeFirstTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ThreeSecondTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TwoFirstTeam",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TwoSecondTeam",
                table: "Scores");

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstTeamScore = table.Column<int>(type: "INTEGER", nullable: false),
                    ScoreId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondTeamScore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Scores_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "Scores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ScoreId",
                table: "Sets",
                column: "ScoreId");
        }
    }
}
