using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlamningsuppgift.Migrations.Data
{
    /// <inheritdoc />
    public partial class Addeddescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ProductDescriptionLong",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescriptionShort",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDescriptionLong",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDescriptionShort",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArticleNumber",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
