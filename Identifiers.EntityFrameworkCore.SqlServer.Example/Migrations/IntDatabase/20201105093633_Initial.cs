using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identifiers.EntityFrameworkCore.SqlServer.Example.Migrations.IntDatabase
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExampleGuidEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Test = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleGuidEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExampleIdentifierEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleIdentifierEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExampleIntEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleIntEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExampleLongEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleLongEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExampleGuidEntityTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExampleGuidEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleGuidEntityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleGuidEntityTranslation_ExampleGuidEntities_ExampleGuidEntityId",
                        column: x => x.ExampleGuidEntityId,
                        principalTable: "ExampleGuidEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleIdentifierEntityTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExampleIdentifierEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleIdentifierEntityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleIdentifierEntityTranslation_ExampleIdentifierEntities_ExampleIdentifierEntityId",
                        column: x => x.ExampleIdentifierEntityId,
                        principalTable: "ExampleIdentifierEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleIntEntityTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExampleIntEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleIntEntityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleIntEntityTranslation_ExampleIntEntities_ExampleIntEntityId",
                        column: x => x.ExampleIntEntityId,
                        principalTable: "ExampleIntEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleLongEntityTranslation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExampleLongEntityId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleLongEntityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleLongEntityTranslation_ExampleLongEntities_ExampleLongEntityId",
                        column: x => x.ExampleLongEntityId,
                        principalTable: "ExampleLongEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleGuidEntityTranslation_ExampleGuidEntityId",
                table: "ExampleGuidEntityTranslation",
                column: "ExampleGuidEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleIdentifierEntityTranslation_ExampleIdentifierEntityId",
                table: "ExampleIdentifierEntityTranslation",
                column: "ExampleIdentifierEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleIntEntityTranslation_ExampleIntEntityId",
                table: "ExampleIntEntityTranslation",
                column: "ExampleIntEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleLongEntityTranslation_ExampleLongEntityId",
                table: "ExampleLongEntityTranslation",
                column: "ExampleLongEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleGuidEntityTranslation");

            migrationBuilder.DropTable(
                name: "ExampleIdentifierEntityTranslation");

            migrationBuilder.DropTable(
                name: "ExampleIntEntityTranslation");

            migrationBuilder.DropTable(
                name: "ExampleLongEntityTranslation");

            migrationBuilder.DropTable(
                name: "ExampleGuidEntities");

            migrationBuilder.DropTable(
                name: "ExampleIdentifierEntities");

            migrationBuilder.DropTable(
                name: "ExampleIntEntities");

            migrationBuilder.DropTable(
                name: "ExampleLongEntities");
        }
    }
}
