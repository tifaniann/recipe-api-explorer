using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheMealDBApp.Migrations
{
    /// <inheritdoc />
    public partial class statusDummy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "dummys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "dummys");
        }
    }
}
