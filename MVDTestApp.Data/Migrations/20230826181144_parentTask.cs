using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVDTestApp.Data.Migrations
{
    public partial class parentTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_WorkTaskId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "WorkTaskId",
                table: "Tasks",
                newName: "ParentWorkTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_WorkTaskId",
                table: "Tasks",
                newName: "IX_Tasks_ParentWorkTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_ParentWorkTaskId",
                table: "Tasks",
                column: "ParentWorkTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_ParentWorkTaskId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ParentWorkTaskId",
                table: "Tasks",
                newName: "WorkTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ParentWorkTaskId",
                table: "Tasks",
                newName: "IX_Tasks_WorkTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_WorkTaskId",
                table: "Tasks",
                column: "WorkTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
