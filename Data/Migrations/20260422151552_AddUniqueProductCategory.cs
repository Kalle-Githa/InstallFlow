using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstallFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId_ProductId",
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_CategoryId_ProductId",
                table: "ProductCategories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");
        }
    }
}
