using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFAE.Jobs.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Applicant_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Applicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalityId",
                table: "Applicants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoNumeric = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsoAlpha = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UniversalCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UniversalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Governorates_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    UniversalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CountryId",
                table: "Applicants",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_GovernorateId",
                table: "Applicants",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_LocalityId",
                table: "Applicants",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_CountryId",
                table: "Governorates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Localities_GovernorateId",
                table: "Localities",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Countries_CountryId",
                table: "Applicants",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Governorates_GovernorateId",
                table: "Applicants",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Localities_LocalityId",
                table: "Applicants",
                column: "LocalityId",
                principalTable: "Localities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Countries_CountryId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Governorates_GovernorateId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Localities_LocalityId",
                table: "Applicants");

            migrationBuilder.DropTable(
                name: "Localities");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_CountryId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_GovernorateId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_LocalityId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "LocalityId",
                table: "Applicants");
        }
    }
}
