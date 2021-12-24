using Microsoft.EntityFrameworkCore.Migrations;

namespace EdutonPetrpku.Data.Migrations
{
    public partial class AddDiplomaForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities");

            migrationBuilder.AddColumn<string>(
                name: "Diploma",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "Diploma",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities",
                column: "AppUserId");
        }
    }
}
