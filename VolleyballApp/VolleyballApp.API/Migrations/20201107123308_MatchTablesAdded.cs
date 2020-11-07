using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class MatchTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstTeamScore = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondTeamScore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstTeamSets = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondTeamSets = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstSetId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondSetId = table.Column<int>(type: "INTEGER", nullable: true),
                    ThirdSetId = table.Column<int>(type: "INTEGER", nullable: true),
                    FourthSetId = table.Column<int>(type: "INTEGER", nullable: true),
                    FifthSetId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Sets_FifthSetId",
                        column: x => x.FifthSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Sets_FirstSetId",
                        column: x => x.FirstSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Sets_FourthSetId",
                        column: x => x.FourthSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Sets_SecondSetId",
                        column: x => x.SecondSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Sets_ThirdSetId",
                        column: x => x.ThirdSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstTeamId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondTeamId = table.Column<int>(type: "INTEGER", nullable: true),
                    ScoreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Scores_ScoreId",
                        column: x => x.ScoreId,
                        principalTable: "Scores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_FirstTeamId",
                        column: x => x.FirstTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_SecondTeamId",
                        column: x => x.SecondTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FirstTeamId",
                table: "Matches",
                column: "FirstTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ScoreId",
                table: "Matches",
                column: "ScoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SecondTeamId",
                table: "Matches",
                column: "SecondTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_FifthSetId",
                table: "Scores",
                column: "FifthSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_FirstSetId",
                table: "Scores",
                column: "FirstSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_FourthSetId",
                table: "Scores",
                column: "FourthSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_SecondSetId",
                table: "Scores",
                column: "SecondSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_ThirdSetId",
                table: "Scores",
                column: "ThirdSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Sets");
        }
    }
}
