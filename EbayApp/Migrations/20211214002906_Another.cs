using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EbayApp.Migrations
{
    public partial class Another : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Enrollment",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Course",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Course");
        }
    }
}
