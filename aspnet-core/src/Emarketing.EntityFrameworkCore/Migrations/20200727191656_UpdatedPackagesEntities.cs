using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedPackagesEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyAdCount",
                table: "Packages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationInDays",
                table: "Packages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ReferralAmount",
                table: "Packages",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalEarning",
                table: "Packages",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyAdCount",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ReferralAmount",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "TotalEarning",
                table: "Packages");
        }
    }
}
