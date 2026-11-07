using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeLocalStorage.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class V08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Materials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Materials",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
