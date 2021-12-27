using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdutonPetrpku.Data.Migrations
{
    public partial class AddPdfUrlToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "AspNetUsers");
        }
    }
}
