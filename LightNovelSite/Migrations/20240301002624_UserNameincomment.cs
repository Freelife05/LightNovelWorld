using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightNovelSite.Migrations
{
    public partial class UserNameincomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Comments",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Comments",
                newName: "UserID");
        }
    }
}
