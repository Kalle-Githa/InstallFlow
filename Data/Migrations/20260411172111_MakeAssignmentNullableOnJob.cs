using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstallFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeAssignmentNullableOnJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Assignments_AssignmentId",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "Jobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Assignments_AssignmentId",
                table: "Jobs",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Assignments_AssignmentId",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Assignments_AssignmentId",
                table: "Jobs",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
