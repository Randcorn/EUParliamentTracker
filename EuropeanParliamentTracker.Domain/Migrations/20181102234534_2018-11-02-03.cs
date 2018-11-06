using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    public partial class _2018110203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");
        }
    }
}
