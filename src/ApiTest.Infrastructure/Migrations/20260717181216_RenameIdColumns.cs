using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InventoryMovements",
                newName: "MovementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovementId",
                table: "InventoryMovements",
                newName: "Id");
        }
    }
}
