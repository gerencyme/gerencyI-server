using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreationReleationship7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_STAND_AspNetUsers_CompanyId",
                table: "STAND");

            migrationBuilder.DropIndex(
                name: "IX_STAND_CompanyId",
                table: "STAND");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "STAND");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "STAND",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_STAND_CompanyId",
                table: "STAND",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_STAND_AspNetUsers_CompanyId",
                table: "STAND",
                column: "CompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
