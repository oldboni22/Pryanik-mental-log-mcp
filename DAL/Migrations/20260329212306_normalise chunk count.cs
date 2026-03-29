using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class normalisechunkcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalChunks",
                table: "EntryChunks");

            migrationBuilder.AddColumn<int>(
                name: "TotalChunks",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalChunks",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "TotalChunks",
                table: "EntryChunks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
