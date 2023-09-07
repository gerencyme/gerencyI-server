using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreationReleationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StandId",
                table: "DEMAND",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PRODUCT",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description_product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_PRODUCT_AspNetUsers_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "STAND",
                columns: table => new
                {
                    StandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stand_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stand_all_read_reserved = table.Column<bool>(type: "bit", nullable: false),
                    user_permission = table.Column<bool>(type: "bit", nullable: false),
                    cpf_responsible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payment_total = table.Column<float>(type: "real", nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StandStatus = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAND", x => x.StandId);
                    table.ForeignKey(
                        name: "FK_STAND_AspNetUsers_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEMAND_StandId",
                table: "DEMAND",
                column: "StandId");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_CompanyId",
                table: "PRODUCT",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_STAND_CompanyId",
                table: "STAND",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DEMAND_STAND_StandId",
                table: "DEMAND",
                column: "StandId",
                principalTable: "STAND",
                principalColumn: "StandId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DEMAND_STAND_StandId",
                table: "DEMAND");

            migrationBuilder.DropTable(
                name: "PRODUCT");

            migrationBuilder.DropTable(
                name: "STAND");

            migrationBuilder.DropIndex(
                name: "IX_DEMAND_StandId",
                table: "DEMAND");

            migrationBuilder.DropColumn(
                name: "StandId",
                table: "DEMAND");
        }
    }
}
