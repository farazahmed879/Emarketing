using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class updatedUserRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_UserId",
                table: "UserRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_AbpUsers_UserId",
                table: "UserRequests",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_AbpUsers_UserId",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_UserId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRequests");
        }
    }
}
