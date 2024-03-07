using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMlibraryApp.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class Addtags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tag",
                table: "Books",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag",
                table: "Books");
        }
    }
}
