using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedPackgeEntityForLimitValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUnlimited",
                table: "Packages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Limit",
                table: "Packages",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumWithdraw",
                table: "Packages",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumWithdraw",
                table: "Packages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUnlimited",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Limit",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "MaximumWithdraw",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "MinimumWithdraw",
                table: "Packages");
        }
    }
}
