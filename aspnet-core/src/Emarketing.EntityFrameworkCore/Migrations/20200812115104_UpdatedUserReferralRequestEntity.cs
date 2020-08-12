using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedUserReferralRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserReferralId",
                table: "UserReferralRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReferralRequests_UserReferralId",
                table: "UserReferralRequests",
                column: "UserReferralId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReferralRequests_AbpUsers_UserReferralId",
                table: "UserReferralRequests",
                column: "UserReferralId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReferralRequests_AbpUsers_UserReferralId",
                table: "UserReferralRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserReferralRequests_UserReferralId",
                table: "UserReferralRequests");

            migrationBuilder.DropColumn(
                name: "UserReferralId",
                table: "UserReferralRequests");
        }
    }
}
