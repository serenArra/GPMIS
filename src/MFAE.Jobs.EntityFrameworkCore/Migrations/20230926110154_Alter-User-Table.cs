using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFAE.Jobs.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Applicants",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentNo",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdentificationTypeId",
                table: "AbpUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_UserId",
                table: "Applicants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_IdentificationTypeId",
                table: "AbpUsers",
                column: "IdentificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_IdentificationTypes_IdentificationTypeId",
                table: "AbpUsers",
                column: "IdentificationTypeId",
                principalTable: "IdentificationTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_AbpUsers_UserId",
                table: "Applicants",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_IdentificationTypes_IdentificationTypeId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_AbpUsers_UserId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_UserId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_IdentificationTypeId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "DocumentNo",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IdentificationTypeId",
                table: "AbpUsers");
        }
    }
}
