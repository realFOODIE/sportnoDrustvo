using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sportnoDrustvo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clani",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", nullable: false),
                    Priimek = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clani", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Termini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImeEkipe = table.Column<string>(type: "TEXT", nullable: false),
                    DatumTermina = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaxUdelezencev = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termini", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trenerji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", nullable: false),
                    Specializacija = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trenerji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rekviziti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", nullable: false),
                    JeNaVoljo = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClanId = table.Column<int>(type: "INTEGER", nullable: true),
                    DatumIzposoje = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rekviziti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rekviziti_Clani_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clani",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Obvestila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TerminId = table.Column<int>(type: "INTEGER", nullable: false),
                    Obvescanje = table.Column<bool>(type: "INTEGER", nullable: false),
                    ClanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obvestila", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obvestila_Clani_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clani",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Obvestila_Termini_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TerminId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClanId = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumRezervacije = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Clani_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clani",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Termini_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Obvestila_ClanId",
                table: "Obvestila",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Obvestila_TerminId",
                table: "Obvestila",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Rekviziti_ClanId",
                table: "Rekviziti",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_ClanId",
                table: "Rezervacije",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_TerminId",
                table: "Rezervacije",
                column: "TerminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obvestila");

            migrationBuilder.DropTable(
                name: "Rekviziti");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Trenerji");

            migrationBuilder.DropTable(
                name: "Clani");

            migrationBuilder.DropTable(
                name: "Termini");
        }
    }
}
