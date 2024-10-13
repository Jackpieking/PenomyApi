using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PenomyAPI.Persist.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class M2_Add_missing_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "penomy_artwork_view_statistic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    TotalViews = table.Column<long>(type: "bigint", nullable: false),
                    From = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    To = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_view_statistic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_guest_artwork_view_history",
                columns: table => new
                {
                    GuestId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkType = table.Column<int>(type: "integer", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_guest_artwork_view_history", x => new { x.GuestId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_guest_artwork_view_history_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_guest_artwork_view_history_penomy_artwork_chapter_Ch~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_guest_tracking",
                columns: table => new
                {
                    GuestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastActiveAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_guest_tracking", x => x.GuestId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_view_statistic_ArtworkId",
                table: "penomy_artwork_view_statistic",
                column: "ArtworkId")
                .Annotation("Npgsql:CreatedConcurrently", true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_guest_artwork_view_history_ArtworkId",
                table: "penomy_guest_artwork_view_history",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_guest_artwork_view_history_ArtworkType",
                table: "penomy_guest_artwork_view_history",
                column: "ArtworkType");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_guest_artwork_view_history_ChapterId",
                table: "penomy_guest_artwork_view_history",
                column: "ChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "penomy_artwork_view_statistic");

            migrationBuilder.DropTable(
                name: "penomy_guest_artwork_view_history");

            migrationBuilder.DropTable(
                name: "penomy_guest_tracking");
        }
    }
}
