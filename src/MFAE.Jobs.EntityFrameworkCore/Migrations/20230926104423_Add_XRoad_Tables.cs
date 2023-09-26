using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MFAE.Jobs.Migrations
{
    /// <inheritdoc />
    public partial class Add_XRoad_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XRoadMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lookup = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SystemId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_XRoadMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XRoadServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProviderCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResultCodePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoapActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProducerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_XRoadServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XRoadServiceAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceAttributeType = table.Column<int>(type: "int", nullable: false),
                    AttributeCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    XMLPath = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FormatTransition = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XRoadServiceID = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_XRoadServiceAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XRoadServiceAttributes_XRoadServices_XRoadServiceID",
                        column: x => x.XRoadServiceID,
                        principalTable: "XRoadServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XRoadServiceErrors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessageAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessageEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    XRoadServiceId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_XRoadServiceErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XRoadServiceErrors_XRoadServices_XRoadServiceId",
                        column: x => x.XRoadServiceId,
                        principalTable: "XRoadServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XRoadServiceAttributeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceAttributeType = table.Column<int>(type: "int", nullable: false),
                    SourceValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttributeID = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_XRoadServiceAttributeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XRoadServiceAttributeMappings_XRoadServiceAttributes_AttributeID",
                        column: x => x.AttributeID,
                        principalTable: "XRoadServiceAttributes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_XRoadServiceAttributeMappings_AttributeID",
                table: "XRoadServiceAttributeMappings",
                column: "AttributeID");

            migrationBuilder.CreateIndex(
                name: "IX_XRoadServiceAttributes_XRoadServiceID",
                table: "XRoadServiceAttributes",
                column: "XRoadServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_XRoadServiceErrors_XRoadServiceId",
                table: "XRoadServiceErrors",
                column: "XRoadServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XRoadMappings");

            migrationBuilder.DropTable(
                name: "XRoadServiceAttributeMappings");

            migrationBuilder.DropTable(
                name: "XRoadServiceErrors");

            migrationBuilder.DropTable(
                name: "XRoadServiceAttributes");

            migrationBuilder.DropTable(
                name: "XRoadServices");
        }
    }
}
