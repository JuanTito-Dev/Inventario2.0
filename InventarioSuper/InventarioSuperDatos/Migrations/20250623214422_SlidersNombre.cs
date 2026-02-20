using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioSuper.Data.Migrations
{
    /// <inheritdoc />
    public partial class SlidersNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Sliders");
        }
    }
}
