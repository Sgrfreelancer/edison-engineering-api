using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdisonEngineering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeadFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Leads");
        }
    }
}
