using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    public partial class _2018110202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parliamentarians_NationalPartys_NationalPartyId",
                table: "Parliamentarians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NationalPartys",
                table: "NationalPartys");

            migrationBuilder.RenameTable(
                name: "NationalPartys",
                newName: "NationalParties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NationalParties",
                table: "NationalParties",
                column: "NationalPartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parliamentarians_NationalParties_NationalPartyId",
                table: "Parliamentarians",
                column: "NationalPartyId",
                principalTable: "NationalParties",
                principalColumn: "NationalPartyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parliamentarians_NationalParties_NationalPartyId",
                table: "Parliamentarians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NationalParties",
                table: "NationalParties");

            migrationBuilder.RenameTable(
                name: "NationalParties",
                newName: "NationalPartys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NationalPartys",
                table: "NationalPartys",
                column: "NationalPartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parliamentarians_NationalPartys_NationalPartyId",
                table: "Parliamentarians",
                column: "NationalPartyId",
                principalTable: "NationalPartys",
                principalColumn: "NationalPartyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
