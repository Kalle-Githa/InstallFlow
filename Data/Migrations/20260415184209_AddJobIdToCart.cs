using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstallFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJobIdToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_JobId",
                table: "Carts",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Jobs_JobId",
                table: "Carts",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Jobs_JobId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_JobId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Carts");
        }
    }
}
