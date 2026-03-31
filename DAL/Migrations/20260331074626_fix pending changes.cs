using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixpendingchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelation_Entries_EntryId",
                table: "TraitEntryRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelation_Traits_TraitId",
                table: "TraitEntryRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TraitEntryRelation",
                table: "TraitEntryRelation");

            migrationBuilder.RenameTable(
                name: "TraitEntryRelation",
                newName: "TraitEntryRelations");

            migrationBuilder.RenameIndex(
                name: "IX_TraitEntryRelation_EntryId",
                table: "TraitEntryRelations",
                newName: "IX_TraitEntryRelations_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraitEntryRelations",
                table: "TraitEntryRelations",
                columns: new[] { "TraitId", "EntryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelations_Entries_EntryId",
                table: "TraitEntryRelations",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelations_Traits_TraitId",
                table: "TraitEntryRelations",
                column: "TraitId",
                principalTable: "Traits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelations_Entries_EntryId",
                table: "TraitEntryRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelations_Traits_TraitId",
                table: "TraitEntryRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TraitEntryRelations",
                table: "TraitEntryRelations");

            migrationBuilder.RenameTable(
                name: "TraitEntryRelations",
                newName: "TraitEntryRelation");

            migrationBuilder.RenameIndex(
                name: "IX_TraitEntryRelations_EntryId",
                table: "TraitEntryRelation",
                newName: "IX_TraitEntryRelation_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraitEntryRelation",
                table: "TraitEntryRelation",
                columns: new[] { "TraitId", "EntryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelation_Entries_EntryId",
                table: "TraitEntryRelation",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelation_Traits_TraitId",
                table: "TraitEntryRelation",
                column: "TraitId",
                principalTable: "Traits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
