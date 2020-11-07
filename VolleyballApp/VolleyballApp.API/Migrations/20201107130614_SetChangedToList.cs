using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class SetChangedToList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Sets_FifthSetId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Sets_FirstSetId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Sets_FourthSetId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Sets_SecondSetId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Sets_ThirdSetId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_FifthSetId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_FirstSetId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_FourthSetId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_SecondSetId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_ThirdSetId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FifthSetId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FirstSetId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "FourthSetId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "SecondSetId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ThirdSetId",
                table: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "ScoreId",
                table: "Sets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ScoreId",
                table: "Sets",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Scores_ScoreId",
                table: "Sets",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Scores_ScoreId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_ScoreId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "FifthSetId",
                table: "Scores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstSetId",
                table: "Scores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FourthSetId",
                table: "Scores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondSetId",
                table: "Scores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThirdSetId",
                table: "Scores",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Sets_FifthSetId",
                table: "Scores",
                column: "FifthSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Sets_FirstSetId",
                table: "Scores",
                column: "FirstSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Sets_FourthSetId",
                table: "Scores",
                column: "FourthSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Sets_SecondSetId",
                table: "Scores",
                column: "SecondSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Sets_ThirdSetId",
                table: "Scores",
                column: "ThirdSetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
