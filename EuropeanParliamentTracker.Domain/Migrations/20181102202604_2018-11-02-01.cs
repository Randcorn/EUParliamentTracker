using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EuropeanParliamentTracker.Domain.Migrations
{
    public partial class _2018110201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "NationalPartys",
                columns: table => new
                {
                    NationalPartyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalPartys", x => x.NationalPartyId);
                });

            migrationBuilder.CreateTable(
                name: "Parliamentarians",
                columns: table => new
                {
                    ParliamentarianId = table.Column<Guid>(nullable: false),
                    OfficalId = table.Column<string>(nullable: true),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: false),
                    NationalPartyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parliamentarians", x => x.ParliamentarianId);
                    table.ForeignKey(
                        name: "FK_Parliamentarians_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parliamentarians_NationalPartys_NationalPartyId",
                        column: x => x.NationalPartyId,
                        principalTable: "NationalPartys",
                        principalColumn: "NationalPartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parliamentarians_CountryId",
                table: "Parliamentarians",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parliamentarians_NationalPartyId",
                table: "Parliamentarians",
                column: "NationalPartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parliamentarians");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "NationalPartys");
        }
    }
}
