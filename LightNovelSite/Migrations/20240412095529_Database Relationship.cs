using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightNovelSite.Migrations
{
    public partial class DatabaseRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comments_ChapterId",
                table: "Comments",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_NovelId",
                table: "Chapter",
                column: "NovelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Novels_NovelId",
                table: "Chapter",
                column: "NovelId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Chapter_ChapterId",
                table: "Comments",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Novels_NovelId",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Chapter_ChapterId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ChapterId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_NovelId",
                table: "Chapter");
        }
    }
}
