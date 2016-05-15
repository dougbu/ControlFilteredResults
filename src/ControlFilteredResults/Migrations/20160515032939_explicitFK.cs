using Microsoft.EntityFrameworkCore.Migrations;

namespace ControlFilteredResults.Migrations
{
    public partial class explicitFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_FileList_FileListId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_FileListId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "FileListId",
                table: "File");

            migrationBuilder.AddColumn<int>(
                name: "BackReferenceId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_BackReferenceId",
                table: "File",
                column: "BackReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_FileList_BackReferenceId",
                table: "File",
                column: "BackReferenceId",
                principalTable: "FileList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_FileList_BackReferenceId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_BackReferenceId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "BackReferenceId",
                table: "File");

            migrationBuilder.AddColumn<int>(
                name: "FileListId",
                table: "File",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_FileListId",
                table: "File",
                column: "FileListId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_FileList_FileListId",
                table: "File",
                column: "FileListId",
                principalTable: "FileList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
