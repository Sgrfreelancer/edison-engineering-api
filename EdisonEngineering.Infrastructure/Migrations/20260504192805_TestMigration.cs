using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdisonEngineering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subsidies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinKW = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxKW = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubsidyAmountPerKW = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subsidies");
        }
    }
}
