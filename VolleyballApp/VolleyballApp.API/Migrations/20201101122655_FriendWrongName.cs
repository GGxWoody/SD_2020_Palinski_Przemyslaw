using Microsoft.EntityFrameworkCore.Migrations;

namespace VolleyballApp.API.Migrations
{
    public partial class FriendWrongName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendlist_Users_SecoundUserId",
                table: "Friendlist");

            migrationBuilder.RenameColumn(
                name: "SecoundUserId",
                table: "Friendlist",
                newName: "SecondUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendlist_SecoundUserId",
                table: "Friendlist",
                newName: "IX_Friendlist_SecondUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendlist_Users_SecondUserId",
                table: "Friendlist",
                column: "SecondUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendlist_Users_SecondUserId",
                table: "Friendlist");

            migrationBuilder.RenameColumn(
                name: "SecondUserId",
                table: "Friendlist",
                newName: "SecoundUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendlist_SecondUserId",
                table: "Friendlist",
                newName: "IX_Friendlist_SecoundUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendlist_Users_SecoundUserId",
                table: "Friendlist",
                column: "SecoundUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
