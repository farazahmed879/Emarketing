using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdateUserPackageAdDetailEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPackageAdDetails_PackageAds_PackageAdId",
                table: "UserPackageAdDetails");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UserPackageAdDetails");

            migrationBuilder.AlterColumn<long>(
                name: "PackageAdId",
                table: "UserPackageAdDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPackageAdDetails_PackageAds_PackageAdId",
                table: "UserPackageAdDetails",
                column: "PackageAdId",
                principalTable: "PackageAds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPackageAdDetails_PackageAds_PackageAdId",
                table: "UserPackageAdDetails");

            migrationBuilder.AlterColumn<long>(
                name: "PackageAdId",
                table: "UserPackageAdDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "UserPackageAdDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPackageAdDetails_PackageAds_PackageAdId",
                table: "UserPackageAdDetails",
                column: "PackageAdId",
                principalTable: "PackageAds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
