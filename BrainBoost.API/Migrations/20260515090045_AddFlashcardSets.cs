using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainBoost.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFlashcardSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlashcardSetId",
                table: "Flashcards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlashcardSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardSets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlashcardSets_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_CategoryId",
                table: "FlashcardSets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_CreatedByUserId",
                table: "FlashcardSets",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardSets");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "FlashcardSetId",
                table: "Flashcards");
        }
    }
}
