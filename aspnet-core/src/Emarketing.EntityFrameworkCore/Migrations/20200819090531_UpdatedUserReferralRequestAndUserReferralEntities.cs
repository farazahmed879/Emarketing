using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedUserReferralRequestAndUserReferralEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserReferralRequestId",
                table: "UserReferrals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserReferralRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReferrals_UserReferralRequestId",
                table: "UserReferrals",
                column: "UserReferralRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReferrals_UserReferralRequests_UserReferralRequestId",
                table: "UserReferrals",
                column: "UserReferralRequestId",
                principalTable: "UserReferralRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReferrals_UserReferralRequests_UserReferralRequestId",
                table: "UserReferrals");

            migrationBuilder.DropIndex(
                name: "IX_UserReferrals_UserReferralRequestId",
                table: "UserReferrals");

            migrationBuilder.DropColumn(
                name: "UserReferralRequestId",
                table: "UserReferrals");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserReferralRequests");
        }
    }
}
