﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlamningsuppgift.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddedProductImageName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImageName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageName",
                table: "Products");
        }
    }
}
