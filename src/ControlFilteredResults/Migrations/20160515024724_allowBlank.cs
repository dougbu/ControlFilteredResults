using Microsoft.EntityFrameworkCore.Migrations;

namespace ControlFilteredResults.Migrations
{
    public partial class allowBlank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "File",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "File",
                nullable: false);
        }
    }
}
