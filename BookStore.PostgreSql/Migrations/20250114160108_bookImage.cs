using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class bookImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Books",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Books");
        }
    }
}
