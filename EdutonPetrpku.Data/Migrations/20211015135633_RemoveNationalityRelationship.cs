using Microsoft.EntityFrameworkCore.Migrations;

namespace EdutonPetrpku.Data.Migrations
{
    public partial class RemoveNationalityRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_AppUserId",
                table: "Nationalities",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }
    }
}
