using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PenomyAPI.Persist.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class M4_Add_IsRemoved_Flag_for_CommentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "penomy_user_post_comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "penomy_group_post_comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "penomy_artwork_comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "penomy_user_post_comment");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "penomy_group_post_comment");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "penomy_artwork_comment");
        }
    }
}
