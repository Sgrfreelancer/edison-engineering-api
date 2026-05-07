using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdisonEngineering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPerKW = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityPricings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityPricings");
        }
    }
}
