using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PenomyAPI.Persist.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class M3_Add_RequestStatus_for_Social_Media_Request_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateNickNameAt",
                table: "penomy_user_profile",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "penomy_user_friend_request",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "penomy_user_friend_request",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "penomy_social_group_join_request",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "penomy_social_group_join_request",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "penomy_chat_group_join_request",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "penomy_chat_group_join_request",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_artwork_comment_penomy_artwork_comment_Com~",
                table: "penomy_user_like_artwork_comment",
                column: "CommentId",
                principalTable: "penomy_artwork_comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_artwork_comment_penomy_artwork_comment_Com~",
                table: "penomy_user_like_artwork_comment");

            migrationBuilder.DropColumn(
                name: "UpdateNickNameAt",
                table: "penomy_user_profile");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "penomy_user_friend_request");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "penomy_user_friend_request");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "penomy_social_group_join_request");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "penomy_social_group_join_request");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "penomy_chat_group_join_request");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "penomy_chat_group_join_request");
        }
    }
}
