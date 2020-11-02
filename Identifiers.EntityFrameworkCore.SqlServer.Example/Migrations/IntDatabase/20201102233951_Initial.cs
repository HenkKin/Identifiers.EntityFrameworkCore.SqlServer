using Microsoft.EntityFrameworkCore.Migrations;

namespace Identifiers.EntityFrameworkCore.SqlServer.Example.Migrations.IntDatabase
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExampleEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExampleEntityTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocaleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranslatedEntityId1 = table.Column<int>(type: "int", nullable: true),
                    TranslatedEntityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleEntityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleEntityTranslation_ExampleEntities_TranslatedEntityId1",
                        column: x => x.TranslatedEntityId1,
                        principalTable: "ExampleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExampleEntityTranslation_TranslatedEntityId1",
                table: "ExampleEntityTranslation",
                column: "TranslatedEntityId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleEntityTranslation");

            migrationBuilder.DropTable(
                name: "ExampleEntities");
        }
    }
}
