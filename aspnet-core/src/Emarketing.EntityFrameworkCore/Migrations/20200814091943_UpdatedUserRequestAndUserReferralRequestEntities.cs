using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedUserRequestAndUserReferralRequestEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "UserRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "UserRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "UserReferralRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "UserReferralRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "UserReferralRequests");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "UserReferralRequests");
        }
    }
}
