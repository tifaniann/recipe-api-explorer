using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheMealDBApp.Migrations
{
    /// <inheritdoc />
    public partial class categories_temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories_Temp",
                columns: table => new
                {
                    IdCust = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCategory = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StrCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jml = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories_Temp", x => new { x.IdCust, x.IdCategory });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories_Temp");
        }
    }
}
