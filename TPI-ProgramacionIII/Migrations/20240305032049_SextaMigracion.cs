using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPI_ProgramacionIII.Migrations
{
    /// <inheritdoc />
    public partial class SextaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "LinesOfOrder",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "LinesOfOrder");
        }
    }
}
