using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFAE.Jobs.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Advertisement_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdvertisementDate",
                table: "JobAdvertisements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AdvertisementId",
                table: "JobAdvertisements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "AllowedAge",
                table: "JobAdvertisements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "JobAdvertisements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "JobAdvertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "JobAdvertisements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "DocumentNo",
                table: "Applicants",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertisementDate",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "AllowedAge",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "JobAdvertisements");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentNo",
                table: "Applicants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);
        }
    }
}
