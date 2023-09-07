using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreationReleationship4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DEMAND_STAND_StandId",
                table: "DEMAND");

            migrationBuilder.DropIndex(
                name: "IX_DEMAND_StandId",
                table: "DEMAND");

            migrationBuilder.DropColumn(
                name: "StandId",
                table: "DEMAND");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StandId",
                table: "DEMAND",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DEMAND_StandId",
                table: "DEMAND",
                column: "StandId");

            migrationBuilder.AddForeignKey(
                name: "FK_DEMAND_STAND_StandId",
                table: "DEMAND",
                column: "StandId",
                principalTable: "STAND",
                principalColumn: "StandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
