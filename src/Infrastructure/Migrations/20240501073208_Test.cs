using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "Portal",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ItemOrdered_ProductUrl",
                table: "OrderItem",
                newName: "ItemOrdered_PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Portal",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "ItemOrdered_PictureUrl",
                table: "OrderItem",
                newName: "ItemOrdered_ProductUrl");
        }
    }
}
