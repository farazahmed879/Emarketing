using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class AddedUserPackageAdDetailEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPackageAdDetails",
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
                    UserId = table.Column<long>(nullable: false),
                    UserPackageSubscriptionDetailId = table.Column<long>(nullable: false),
                    PackageAdId = table.Column<long>(nullable: true),
                    PackageId = table.Column<long>(nullable: false),
                    AdPrice = table.Column<decimal>(nullable: false),
                    AdDate = table.Column<DateTime>(nullable: false),
                    IsViewed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPackageAdDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPackageAdDetails_PackageAds_PackageAdId",
                        column: x => x.PackageAdId,
                        principalTable: "PackageAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPackageAdDetails_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPackageAdDetails_UserPackageSubscriptionDetails_UserPackageSubscriptionDetailId",
                        column: x => x.UserPackageSubscriptionDetailId,
                        principalTable: "UserPackageSubscriptionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPackageAdDetails_PackageAdId",
                table: "UserPackageAdDetails",
                column: "PackageAdId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackageAdDetails_UserId",
                table: "UserPackageAdDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackageAdDetails_UserPackageSubscriptionDetailId",
                table: "UserPackageAdDetails",
                column: "UserPackageSubscriptionDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPackageAdDetails");
        }
    }
}
