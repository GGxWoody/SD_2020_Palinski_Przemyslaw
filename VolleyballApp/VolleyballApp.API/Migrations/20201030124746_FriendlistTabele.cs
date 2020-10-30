using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class FriendlistTabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Friendlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecoundUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendlist_Users_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendlist_Users_SecoundUserId",
                        column: x => x.SecoundUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendlist_FirstUserId",
                table: "Friendlist",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendlist_SecoundUserId",
                table: "Friendlist",
                column: "SecoundUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendlist");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
