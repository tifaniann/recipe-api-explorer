using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheMealDBApp.Migrations
{
    /// <inheritdoc />
    public partial class Dummy_Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dummys",
                columns: table => new
                {
                    nama_user = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dummys", x => x.nama_user);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dummys");
        }
    }
}
