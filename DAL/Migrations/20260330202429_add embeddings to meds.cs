using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class addembeddingstomeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelation_Trait_TraitId",
                table: "TraitEntryRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trait",
                table: "Trait");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medication",
                table: "Medication");

            migrationBuilder.RenameTable(
                name: "Trait",
                newName: "Traits");

            migrationBuilder.RenameTable(
                name: "Medication",
                newName: "Medications");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Medications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Embedding",
                table: "Medications",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traits",
                table: "Traits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medications",
                table: "Medications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelation_Traits_TraitId",
                table: "TraitEntryRelation",
                column: "TraitId",
                principalTable: "Traits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraitEntryRelation_Traits_TraitId",
                table: "TraitEntryRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traits",
                table: "Traits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medications",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "Medications");

            migrationBuilder.RenameTable(
                name: "Traits",
                newName: "Trait");

            migrationBuilder.RenameTable(
                name: "Medications",
                newName: "Medication");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trait",
                table: "Trait",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medication",
                table: "Medication",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TraitEntryRelation_Trait_TraitId",
                table: "TraitEntryRelation",
                column: "TraitId",
                principalTable: "Trait",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
