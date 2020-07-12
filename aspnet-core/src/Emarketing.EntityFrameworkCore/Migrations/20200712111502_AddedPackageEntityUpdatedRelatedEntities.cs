using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class AddedPackageEntityUpdatedRelatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "UserRequests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "UserReferrals",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "UserReferralRequests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ProfitValue = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_PackageId",
                table: "UserRequests",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReferrals_PackageId",
                table: "UserReferrals",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReferralRequests_PackageId",
                table: "UserReferralRequests",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReferralRequests_Packages_PackageId",
                table: "UserReferralRequests",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReferrals_Packages_PackageId",
                table: "UserReferrals",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_Packages_PackageId",
                table: "UserRequests",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReferralRequests_Packages_PackageId",
                table: "UserReferralRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReferrals_Packages_PackageId",
                table: "UserReferrals");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Packages_PackageId",
                table: "UserRequests");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_PackageId",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserReferrals_PackageId",
                table: "UserReferrals");

            migrationBuilder.DropIndex(
                name: "IX_UserReferralRequests_PackageId",
                table: "UserReferralRequests");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UserReferrals");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UserReferralRequests");
        }
    }
}
