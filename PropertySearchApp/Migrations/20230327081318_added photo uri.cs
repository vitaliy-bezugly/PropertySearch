using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertySearchApp.Migrations
{
    public partial class addedphotouri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUri",
                table: "Accommodation",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUri",
                table: "Accommodation");
        }
    }
}
