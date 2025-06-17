using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductTask.Migrations
{
    /// <inheritdoc />
    public partial class odrerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "checkoutForms2",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "checkoutForms2");
        }
    }
}
