using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pixel_Palace.Migrations
{
    public partial class cat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_categories_Category_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_Category_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Category_id",
                table: "users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_users_Category_id",
                table: "users",
                column: "Category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_categories_Category_id",
                table: "users",
                column: "Category_id",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
