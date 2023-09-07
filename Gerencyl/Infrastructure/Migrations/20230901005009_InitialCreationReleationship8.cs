using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreationReleationship8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STAND");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STAND",
                columns: table => new
                {
                    Stand_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cpf_responsible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment_total = table.Column<float>(type: "real", nullable: false),
                    stand_all_read_reserved = table.Column<bool>(type: "bit", nullable: false),
                    stand_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandStatus = table.Column<int>(type: "int", nullable: false),
                    user_permission = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAND", x => x.Stand_Id);
                });
        }
    }
}
