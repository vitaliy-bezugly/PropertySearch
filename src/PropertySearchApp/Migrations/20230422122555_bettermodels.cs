using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertySearchApp.Migrations
{
    public partial class bettermodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_AspNetUsers_UserId",
                table: "Accommodation");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_AspNetUsers_UserEntityId",
                table: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.RenameTable(
                name: "Accommodation",
                newName: "Accommodations");

            migrationBuilder.RenameColumn(
                name: "UserEntityId",
                table: "Contacts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_UserEntityId",
                table: "Contacts",
                newName: "IX_Contacts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodation_UserId",
                table: "Accommodations",
                newName: "IX_Accommodations_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Information",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Contacts",
                type: "nvarchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactType",
                table: "Contacts",
                type: "nvarchar(64)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Accommodations",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Accommodations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_UserId",
                table: "Accommodations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_AspNetUsers_UserId",
                table: "Contacts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_UserId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AspNetUsers_UserId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_LocationId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Accommodations");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.RenameTable(
                name: "Accommodations",
                newName: "Accommodation");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Contact",
                newName: "UserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_UserId",
                table: "Contact",
                newName: "IX_Contact_UserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_UserId",
                table: "Accommodation",
                newName: "IX_Accommodation_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Information",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactType",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Accommodation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodation_AspNetUsers_UserId",
                table: "Accommodation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_AspNetUsers_UserEntityId",
                table: "Contact",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
