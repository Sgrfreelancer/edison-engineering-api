using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdisonEngineering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElectricitySlab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricitySlabs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinUnit = table.Column<int>(type: "int", nullable: false),
                    MaxUnit = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricitySlabs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricitySlabs");
        }
    }
}
