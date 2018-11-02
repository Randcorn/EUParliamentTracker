using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Migrations
{
    public partial class _2018110202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OfficalId",
                table: "Parliamentarians",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OfficalId",
                table: "Parliamentarians",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
