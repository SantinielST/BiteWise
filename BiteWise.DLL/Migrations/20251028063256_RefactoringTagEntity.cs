using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiteWise.DLL.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringTagEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Tags",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tags",
                newName: "Link");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
