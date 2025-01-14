using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orange.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderHeaderTableIDName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderHeaders",
                newName: "OrderHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderHeaderId",
                table: "OrderHeaders",
                newName: "Id");
        }
    }
}
