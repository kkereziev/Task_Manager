using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Manager.Api.Migrations
{
    public partial class Refactor_Comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WorkerId",
                table: "Comments",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Workers_WorkerId",
                table: "Comments",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Workers_WorkerId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_WorkerId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Comments");
        }
    }
}
