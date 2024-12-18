using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PenomyAPI.Persist.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class M5_Update_FileName_for_media_and_UserLikeValueFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_penomy_chat_message_like_statistic_penomy_user_like_value_L~",
                table: "penomy_chat_message_like_statistic");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_chat_message_penomy_user_like_value_LikeVa~",
                table: "penomy_user_like_chat_message");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_group_post_penomy_user_like_value_LikeValu~",
                table: "penomy_user_like_group_post");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_group_post_comment_penomy_user_like_value_~",
                table: "penomy_user_like_group_post_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_user_post_penomy_user_like_value_LikeValue~",
                table: "penomy_user_like_user_post");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_like_user_post_comment_penomy_user_like_value_L~",
                table: "penomy_user_like_user_post_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_post_comment_like_statistics_penomy_user_like_v~",
                table: "penomy_user_post_comment_like_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_post_like_statistics_penomy_user_like_value_Lik~",
                table: "penomy_user_post_like_statistics");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_like_user_post_comment_LikeValueId",
                table: "penomy_user_like_user_post_comment");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_like_user_post_LikeValueId",
                table: "penomy_user_like_user_post");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_like_group_post_comment_LikeValueId",
                table: "penomy_user_like_group_post_comment");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_like_group_post_LikeValueId",
                table: "penomy_user_like_group_post");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_like_chat_message_LikeValueId",
                table: "penomy_user_like_chat_message");

            migrationBuilder.DropIndex(
                name: "IX_penomy_chat_message_like_statistic_LikeValueId",
                table: "penomy_chat_message_like_statistic");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_user_like_user_post_comment");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_user_like_user_post");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_user_like_group_post_comment");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_user_like_group_post");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_user_like_chat_message");

            migrationBuilder.DropColumn(
                name: "LikeValueId",
                table: "penomy_chat_message_like_statistic");

            migrationBuilder.RenameColumn(
                name: "LikeValueId",
                table: "penomy_user_post_like_statistics",
                newName: "UserLikeValueId");

            migrationBuilder.RenameIndex(
                name: "IX_penomy_user_post_like_statistics_LikeValueId",
                table: "penomy_user_post_like_statistics",
                newName: "IX_penomy_user_post_like_statistics_UserLikeValueId");

            migrationBuilder.RenameColumn(
                name: "LikeValueId",
                table: "penomy_group_post_comment_like_statistic",
                newName: "UserLikeValueId");

            migrationBuilder.RenameIndex(
                name: "IX_penomy_group_post_comment_like_statistic_LikeValueId",
                table: "penomy_group_post_comment_like_statistic",
                newName: "IX_penomy_group_post_comment_like_statistic_UserLikeValueId");

            migrationBuilder.AddColumn<long>(
                name: "UserLikeValueId",
                table: "penomy_user_post_comment_like_statistics",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_user_post_comment_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_user_post_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_group_post_comment_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_group_post_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_chat_message_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_bug_report_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_report_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_chapter_report_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_chapter_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_bug_report_attached_media",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_comment_like_statistics_UserLikeValueId",
                table: "penomy_user_post_comment_like_statistics",
                column: "UserLikeValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_post_comment_like_statistics_penomy_user_like_v~",
                table: "penomy_user_post_comment_like_statistics",
                column: "UserLikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_post_like_statistics_penomy_user_like_value_Use~",
                table: "penomy_user_post_like_statistics",
                column: "UserLikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_post_comment_like_statistics_penomy_user_like_v~",
                table: "penomy_user_post_comment_like_statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_penomy_user_post_like_statistics_penomy_user_like_value_Use~",
                table: "penomy_user_post_like_statistics");

            migrationBuilder.DropIndex(
                name: "IX_penomy_user_post_comment_like_statistics_UserLikeValueId",
                table: "penomy_user_post_comment_like_statistics");

            migrationBuilder.DropColumn(
                name: "UserLikeValueId",
                table: "penomy_user_post_comment_like_statistics");

            migrationBuilder.RenameColumn(
                name: "UserLikeValueId",
                table: "penomy_user_post_like_statistics",
                newName: "LikeValueId");

            migrationBuilder.RenameIndex(
                name: "IX_penomy_user_post_like_statistics_UserLikeValueId",
                table: "penomy_user_post_like_statistics",
                newName: "IX_penomy_user_post_like_statistics_LikeValueId");

            migrationBuilder.RenameColumn(
                name: "UserLikeValueId",
                table: "penomy_group_post_comment_like_statistic",
                newName: "LikeValueId");

            migrationBuilder.RenameIndex(
                name: "IX_penomy_group_post_comment_like_statistic_UserLikeValueId",
                table: "penomy_group_post_comment_like_statistic",
                newName: "IX_penomy_group_post_comment_like_statistic_LikeValueId");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_user_post_comment_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_user_post_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_user_like_user_post_comment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_user_like_user_post",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_user_like_group_post_comment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_user_like_group_post",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_user_like_chat_message",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_group_post_comment_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_group_post_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<long>(
                name: "LikeValueId",
                table: "penomy_chat_message_like_statistic",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_chat_message_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_bug_report_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_report_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_chapter_report_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_chapter_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "penomy_artwork_bug_report_attached_media",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_comment_LikeValueId",
                table: "penomy_user_like_user_post_comment",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_LikeValueId",
                table: "penomy_user_like_user_post",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_comment_LikeValueId",
                table: "penomy_user_like_group_post_comment",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_LikeValueId",
                table: "penomy_user_like_group_post",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_chat_message_LikeValueId",
                table: "penomy_user_like_chat_message",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_message_like_statistic_LikeValueId",
                table: "penomy_chat_message_like_statistic",
                column: "LikeValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_chat_message_like_statistic_penomy_user_like_value_L~",
                table: "penomy_chat_message_like_statistic",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_chat_message_penomy_user_like_value_LikeVa~",
                table: "penomy_user_like_chat_message",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_group_post_penomy_user_like_value_LikeValu~",
                table: "penomy_user_like_group_post",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_group_post_comment_penomy_user_like_value_~",
                table: "penomy_user_like_group_post_comment",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_user_post_penomy_user_like_value_LikeValue~",
                table: "penomy_user_like_user_post",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_like_user_post_comment_penomy_user_like_value_L~",
                table: "penomy_user_like_user_post_comment",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_post_comment_like_statistics_penomy_user_like_v~",
                table: "penomy_user_post_comment_like_statistics",
                column: "CommentId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_penomy_user_post_like_statistics_penomy_user_like_value_Lik~",
                table: "penomy_user_post_like_statistics",
                column: "LikeValueId",
                principalTable: "penomy_user_like_value",
                principalColumn: "Id");
        }
    }
}
