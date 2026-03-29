using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class technicalupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryChunks",
                table: "EntryChunks");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EntryChunks",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Embedding",
                table: "Advices",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryChunks",
                table: "EntryChunks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EntryChunks_EntryId_Number",
                table: "EntryChunks",
                columns: new[] { "EntryId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryChunks",
                table: "EntryChunks");

            migrationBuilder.DropIndex(
                name: "IX_EntryChunks_EntryId_Number",
                table: "EntryChunks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EntryChunks");

            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "Advices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryChunks",
                table: "EntryChunks",
                columns: new[] { "EntryId", "Number" });
        }
    }
}
