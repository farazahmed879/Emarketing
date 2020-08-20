using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarketing.Migrations
{
    public partial class UpdatedWithdrawRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Dated",
                table: "WithdrawRequests",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<long>(
                name: "UserWithdrawDetailId",
                table: "WithdrawRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WithdrawDetails",
                table: "WithdrawRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawRequests_UserWithdrawDetailId",
                table: "WithdrawRequests",
                column: "UserWithdrawDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawRequests_UserWithdrawDetails_UserWithdrawDetailId",
                table: "WithdrawRequests",
                column: "UserWithdrawDetailId",
                principalTable: "UserWithdrawDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawRequests_UserWithdrawDetails_UserWithdrawDetailId",
                table: "WithdrawRequests");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawRequests_UserWithdrawDetailId",
                table: "WithdrawRequests");

            migrationBuilder.DropColumn(
                name: "Dated",
                table: "WithdrawRequests");

            migrationBuilder.DropColumn(
                name: "UserWithdrawDetailId",
                table: "WithdrawRequests");

            migrationBuilder.DropColumn(
                name: "WithdrawDetails",
                table: "WithdrawRequests");
        }
    }
}
