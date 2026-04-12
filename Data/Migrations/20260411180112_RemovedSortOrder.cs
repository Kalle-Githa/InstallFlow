using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstallFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "JobTemplateMaterialRows");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "JobTemplateLaborRows");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "JobMaterialRows");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "JobLaborRows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "JobTemplateMaterialRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "JobTemplateLaborRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "JobMaterialRows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "JobLaborRows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
