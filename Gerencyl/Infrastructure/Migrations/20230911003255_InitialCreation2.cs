using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandProduct_DEMAND_DemandId1",
                table: "DemandProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandProduct_PRODUCT_ProductId1",
                table: "DemandProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemandProduct",
                table: "DemandProduct");

            migrationBuilder.RenameTable(
                name: "DemandProduct",
                newName: "DemandProducts");

            migrationBuilder.RenameIndex(
                name: "IX_DemandProduct_ProductId1",
                table: "DemandProducts",
                newName: "IX_DemandProducts_ProductId1");

            migrationBuilder.RenameIndex(
                name: "IX_DemandProduct_DemandId1",
                table: "DemandProducts",
                newName: "IX_DemandProducts_DemandId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemandProducts",
                table: "DemandProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandProducts_DEMAND_DemandId1",
                table: "DemandProducts",
                column: "DemandId1",
                principalTable: "DEMAND",
                principalColumn: "DemandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandProducts_PRODUCT_ProductId1",
                table: "DemandProducts",
                column: "ProductId1",
                principalTable: "PRODUCT",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandProducts_DEMAND_DemandId1",
                table: "DemandProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_DemandProducts_PRODUCT_ProductId1",
                table: "DemandProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DemandProducts",
                table: "DemandProducts");

            migrationBuilder.RenameTable(
                name: "DemandProducts",
                newName: "DemandProduct");

            migrationBuilder.RenameIndex(
                name: "IX_DemandProducts_ProductId1",
                table: "DemandProduct",
                newName: "IX_DemandProduct_ProductId1");

            migrationBuilder.RenameIndex(
                name: "IX_DemandProducts_DemandId1",
                table: "DemandProduct",
                newName: "IX_DemandProduct_DemandId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DemandProduct",
                table: "DemandProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandProduct_DEMAND_DemandId1",
                table: "DemandProduct",
                column: "DemandId1",
                principalTable: "DEMAND",
                principalColumn: "DemandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandProduct_PRODUCT_ProductId1",
                table: "DemandProduct",
                column: "ProductId1",
                principalTable: "PRODUCT",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
