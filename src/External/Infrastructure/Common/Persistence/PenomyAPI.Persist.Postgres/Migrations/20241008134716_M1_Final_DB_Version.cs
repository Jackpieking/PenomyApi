using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PenomyAPI.Persist.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class M1_Final_DB_Version : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "penomy_chat_message_reply",
                columns: table => new
                {
                    RootChatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    RepliedMessageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_message_reply", x => new { x.RootChatMessageId, x.RepliedMessageId });
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_mail_server_status",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MailDomain = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LeftMailQuota = table.Column<int>(type: "integer", nullable: false),
                    ServerStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_mail_server_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_register_waiting_list",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SendMailStatus = table.Column<int>(type: "integer", nullable: false),
                    LastMailSentAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    NextMailSentAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_register_waiting_list", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_system_account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_system_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_system_role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_system_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_friend",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FriendId = table.Column<long>(type: "bigint", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_friend", x => new { x.UserId, x.FriendId });
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_friend_request",
                columns: table => new
                {
                    FriendId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_friend_request", x => new { x.CreatedBy, x.FriendId });
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_artwork_comment",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_artwork_comment", x => new { x.CommentId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_profile",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NickName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    AvatarUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    AboutMe = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RegisterAsCreator = table.Column<bool>(type: "boolean", nullable: false),
                    TotalFollowedCreators = table.Column<int>(type: "integer", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_profile", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_roleclaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClaimValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_roleclaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_identity_roleclaim_penomy_identity_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "penomy_identity_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_userclaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClaimValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_userclaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_identity_userclaim_penomy_identity_user_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_userlogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_userlogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_penomy_identity_userlogin_penomy_identity_user_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_userrole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_userrole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_penomy_identity_userrole_penomy_identity_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "penomy_identity_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_identity_userrole_penomy_identity_user_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_identity_usertoken",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_identity_usertoken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_penomy_identity_usertoken_penomy_identity_user_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_identity_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_bug_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BugSeverity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_bug_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_type_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_origin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CountryName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Label = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_origin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_origin_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_origin_penomy_system_account_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_report_problem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProblemSeverity = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_report_problem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_problem_penomy_system_account_Created~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_violation_flag_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_violation_flag_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_type_penomy_system_account_Cr~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_type_penomy_system_account_Up~",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_ban_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_ban_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_ban_type_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_bank",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Bin = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    SwiftCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_bank_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_bank_penomy_system_account_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_bug_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BugSeverity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_bug_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_bug_type_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(640)", maxLength: 640, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_category_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_category_penomy_system_account_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_creator_wallet_transaction_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_creator_wallet_transaction_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_creator_wallet_transaction_type_penomy_system_accoun~",
                        column: x => x.CreatorId,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_donation_item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AllowDonatorToSetPrice = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorReceivedPercentage = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_donation_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_donation_item_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_donation_item_penomy_system_account_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_other_info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "character varying(640)", maxLength: 640, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_other_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_other_info_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_post_report_problem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProblemSeverity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_post_report_problem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_post_report_problem_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_revenue_program",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MinTotalViewsToApply = table.Column<int>(type: "integer", nullable: false),
                    MinTotalFollowersToApply = table.Column<int>(type: "integer", nullable: false),
                    MinTotalFavoritesToApply = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_revenue_program", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_revenue_program_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_revenue_program_penomy_system_account_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_report_problem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProblemSeverity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_report_problem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_report_problem_penomy_system_account_Cr~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_value",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    ForDefaultDisplay = table.Column<bool>(type: "boolean", nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmojiUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_like_value_penomy_system_account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_profile_report_problem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProblemSeverity = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_profile_report_problem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_profile_report_problem_penomy_system_account_Cr~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_system_account_role",
                columns: table => new
                {
                    SystemAccountId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_system_account_role", x => new { x.SystemAccountId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_penomy_system_account_role_penomy_system_account_SystemAcco~",
                        column: x => x.SystemAccountId,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_system_account_role_penomy_system_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "penomy_system_role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_comment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsDirectlyCommented = table.Column<bool>(type: "boolean", nullable: false),
                    TotalChildComments = table.Column<int>(type: "integer", nullable: false),
                    TotalLikes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_comment_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ChatGroupType = table.Column<int>(type: "integer", nullable: false),
                    GroupName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CoverPhotoUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    TotalMembers = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_chat_group_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_creator_profile",
                columns: table => new
                {
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    TotalFollowers = table.Column<int>(type: "integer", nullable: false),
                    TotalArtworks = table.Column<int>(type: "integer", nullable: false),
                    ReportedCount = table.Column<int>(type: "integer", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_creator_profile", x => x.CreatorId);
                    table.ForeignKey(
                        name: "FK_penomy_creator_profile_penomy_user_profile_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_granted_authorized_user",
                columns: table => new
                {
                    GrantedTo = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizedUserId = table.Column<long>(type: "bigint", nullable: false),
                    GrantedBy = table.Column<long>(type: "bigint", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_granted_authorized_user", x => new { x.GrantedTo, x.AuthorizedUserId, x.GrantedBy });
                    table.ForeignKey(
                        name: "FK_penomy_granted_authorized_user_penomy_system_account_Grante~",
                        column: x => x.GrantedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_granted_authorized_user_penomy_system_account_Grant~1",
                        column: x => x.GrantedTo,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_granted_authorized_user_penomy_user_profile_Authoriz~",
                        column: x => x.AuthorizedUserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_series",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LastItemOrder = table.Column<int>(type: "integer", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    TemporarilyRemovedBy = table.Column<long>(type: "bigint", nullable: false),
                    TemporarilyRemovedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsTemporarilyRemoved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_series", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_series_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_series_penomy_user_profile_TemporarilyRemovedBy",
                        column: x => x.TemporarilyRemovedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_series_penomy_user_profile_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CoverPhotoUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TotalMembers = table.Column<int>(type: "integer", nullable: false),
                    RequireApprovedWhenPost = table.Column<bool>(type: "boolean", nullable: false),
                    GroupStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_followed_creator",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_followed_creator", x => new { x.UserId, x.CreatorId });
                    table.ForeignKey(
                        name: "FK_penomy_user_followed_creator_penomy_user_profile_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_user_followed_creator_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TotalLikes = table.Column<long>(type: "bigint", nullable: false),
                    PublicLevel = table.Column<int>(type: "integer", nullable: false),
                    AllowComment = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PublicLevel = table.Column<int>(type: "integer", nullable: false),
                    AuthorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HasSeries = table.Column<bool>(type: "boolean", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Introduction = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    LastChapterUploadOrder = table.Column<int>(type: "integer", nullable: false),
                    FixedTotalChapters = table.Column<int>(type: "integer", nullable: false),
                    ArtworkStatus = table.Column<int>(type: "integer", nullable: false),
                    ArtworkType = table.Column<int>(type: "integer", nullable: false),
                    ArtworkOriginId = table.Column<long>(type: "bigint", nullable: false),
                    AllowComment = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsCreatedByAuthorizedUser = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsTemporarilyRemoved = table.Column<bool>(type: "boolean", nullable: false),
                    TemporarilyRemovedBy = table.Column<long>(type: "bigint", nullable: false),
                    TemporarilyRemovedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsTakenDown = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_penomy_artwork_origin_ArtworkOriginId",
                        column: x => x.ArtworkOriginId,
                        principalTable: "penomy_artwork_origin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_penomy_user_profile_TemporarilyRemovedBy",
                        column: x => x.TemporarilyRemovedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_penomy_user_profile_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_ban",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BanTypeId = table.Column<long>(type: "bigint", nullable: false),
                    BannedBy = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_ban", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_ban_penomy_ban_type_BanTypeId",
                        column: x => x.BanTypeId,
                        principalTable: "penomy_ban_type",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_ban_penomy_system_account_BannedBy",
                        column: x => x.BannedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_ban_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_bug_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    BugTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Overview = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    UserDetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveStatus = table.Column<int>(type: "integer", nullable: false),
                    ResolveNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_bug_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_bug_report_penomy_bug_type_BugTypeId",
                        column: x => x.BugTypeId,
                        principalTable: "penomy_bug_type",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_bug_report_penomy_system_account_ResolvedBy",
                        column: x => x.ResolvedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_bug_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_profile_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ReportedProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveNote = table.Column<string>(type: "text", nullable: true),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_profile_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_profile_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_user_profile_report_penomy_user_profile_ReportedProf~",
                        column: x => x.ReportedProfileId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_user_profile_report_penomy_user_profile_report_probl~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_user_profile_report_problem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_comment_parent_child",
                columns: table => new
                {
                    ParentCommentId = table.Column<long>(type: "bigint", nullable: false),
                    ChildCommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_comment_parent_child", x => new { x.ParentCommentId, x.ChildCommentId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_comment_parent_child_penomy_artwork_comment_~",
                        column: x => x.ChildCommentId,
                        principalTable: "penomy_artwork_comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_comment_parent_child_penomy_artwork_comment~1",
                        column: x => x.ParentCommentId,
                        principalTable: "penomy_artwork_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_group_join_request",
                columns: table => new
                {
                    ChatGroupId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_group_join_request", x => new { x.ChatGroupId, x.CreatedBy });
                    table.ForeignKey(
                        name: "FK_penomy_chat_group_join_request_penomy_chat_group_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "penomy_chat_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_chat_group_join_request_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_group_member",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    ChatGroupId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_group_member", x => new { x.MemberId, x.ChatGroupId });
                    table.ForeignKey(
                        name: "FK_penomy_chat_group_member_penomy_chat_group_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "penomy_chat_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_chat_group_member_penomy_user_profile_MemberId",
                        column: x => x.MemberId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    ChatGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ReplyToAnotherMessage = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_chat_message_penomy_chat_group_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "penomy_chat_group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_chat_message_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_chat_group_active_history",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ChatGroupId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_chat_group_active_history", x => new { x.UserId, x.ChatGroupId });
                    table.ForeignKey(
                        name: "FK_penomy_user_chat_group_active_history_penomy_chat_group_Cha~",
                        column: x => x.ChatGroupId,
                        principalTable: "penomy_chat_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_chat_group_active_history_penomy_user_profile_U~",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_creator_wallet",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WalletStatus = table.Column<int>(type: "integer", nullable: false),
                    WalletAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_creator_wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_creator_wallet_penomy_bank_BankId",
                        column: x => x.BankId,
                        principalTable: "penomy_bank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_creator_wallet_penomy_creator_profile_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TotalLikes = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    AllowComment = table.Column<bool>(type: "boolean", nullable: false),
                    PostStatus = table.Column<int>(type: "integer", nullable: false),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_penomy_social_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_penomy_user_profile_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_join_request",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_join_request", x => new { x.GroupId, x.CreatedBy });
                    table.ForeignKey(
                        name: "FK_penomy_social_group_join_request_penomy_social_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_join_request_penomy_user_profile_Create~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_linked_chat_group",
                columns: table => new
                {
                    SocialGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ChatGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_linked_chat_group", x => new { x.SocialGroupId, x.ChatGroupId });
                    table.ForeignKey(
                        name: "FK_penomy_social_group_linked_chat_group_penomy_chat_group_Cha~",
                        column: x => x.ChatGroupId,
                        principalTable: "penomy_chat_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_linked_chat_group_penomy_social_group_S~",
                        column: x => x.SocialGroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_member",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_member", x => new { x.GroupId, x.MemberId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_penomy_social_group_member_penomy_social_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_member_penomy_user_profile_MemberId",
                        column: x => x.MemberId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_report_penomy_social_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_report_penomy_social_group_report_probl~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_social_group_report_problem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_violation_flag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    ResolveNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_violation_flag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_violation_flag_penomy_social_group_Grou~",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_social_group_violation_flag_penomy_system_account_Cr~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_violation_flag_penomy_user_profile_Reso~",
                        column: x => x.ResolvedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_user_post",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_user_post", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_penomy_user_like_value_LikeValue~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_attached_media_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_comment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    IsDirectlyCommented = table.Column<bool>(type: "boolean", nullable: false),
                    TotalChildComments = table.Column<int>(type: "integer", nullable: false),
                    TotalLikes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_like_statistics",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_like_statistics", x => new { x.PostId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_penomy_user_post_like_statistics_penomy_user_like_value_Lik~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_post_like_statistics_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_report_penomy_post_report_problem_ReportPr~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_post_report_problem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_post_report_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_post_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_saved_user_posts",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_saved_user_posts", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_penomy_user_saved_user_posts_penomy_user_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_user_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_saved_user_posts_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_applied_revenue_program",
                columns: table => new
                {
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    RevenueProgramId = table.Column<long>(type: "bigint", nullable: false),
                    AppliedStatus = table.Column<int>(type: "integer", nullable: false),
                    ProposedBy = table.Column<long>(type: "bigint", nullable: false),
                    ProposedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    RejectedNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_applied_revenue_program", x => new { x.ArtworkId, x.RevenueProgramId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_applied_revenue_program_penomy_artwork_Artwo~",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_applied_revenue_program_penomy_creator_profi~",
                        column: x => x.ProposedBy,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_applied_revenue_program_penomy_system_accoun~",
                        column: x => x.ApprovedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_bug_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    BugTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(640)", maxLength: 640, nullable: false),
                    Overview = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    UserDetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveStatus = table.Column<int>(type: "integer", nullable: false),
                    ResolveNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_bug_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_report_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_report_penomy_artwork_bug_type_BugTypeId",
                        column: x => x.BugTypeId,
                        principalTable: "penomy_artwork_bug_type",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_report_penomy_system_account_ResolvedBy",
                        column: x => x.ResolvedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_category",
                columns: table => new
                {
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_category", x => new { x.ArtworkId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_category_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_category_penomy_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "penomy_category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_chapter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    PublicLevel = table.Column<int>(type: "integer", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PublishStatus = table.Column<int>(type: "integer", nullable: false),
                    AllowComment = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    TemporarilyRemovedBy = table.Column<long>(type: "bigint", nullable: false),
                    TemporarilyRemovedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsTemporarilyRemoved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_penomy_user_profile_TemporarilyRemov~",
                        column: x => x.TemporarilyRemovedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_penomy_user_profile_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_comment_reference",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_comment_reference", x => new { x.CommentId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_comment_reference_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_comment_reference_penomy_artwork_comment_Com~",
                        column: x => x.CommentId,
                        principalTable: "penomy_artwork_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_metadata",
                columns: table => new
                {
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    TotalViews = table.Column<long>(type: "bigint", nullable: false),
                    TotalComments = table.Column<long>(type: "bigint", nullable: false),
                    TotalFollowers = table.Column<long>(type: "bigint", nullable: false),
                    TotalFavorites = table.Column<long>(type: "bigint", nullable: false),
                    TotalStarRates = table.Column<long>(type: "bigint", nullable: false),
                    TotalUsersRated = table.Column<long>(type: "bigint", nullable: false),
                    AverageStarRate = table.Column<double>(type: "double precision", nullable: false),
                    HasFanGroup = table.Column<bool>(type: "boolean", nullable: false),
                    HasAdRevenueEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_metadata", x => x.ArtworkId);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_metadata_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_other_info",
                columns: table => new
                {
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    OtherInfoId = table.Column<long>(type: "bigint", nullable: false),
                    InfoName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_other_info", x => new { x.ArtworkId, x.OtherInfoId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_other_info_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_other_info_penomy_other_info_OtherInfoId",
                        column: x => x.OtherInfoId,
                        principalTable: "penomy_other_info",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveStatus = table.Column<int>(type: "integer", nullable: false),
                    ResolveNote = table.Column<string>(type: "text", nullable: true),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResolverCreatorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_penomy_artwork_report_problem_ReportP~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_artwork_report_problem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_penomy_creator_profile_ResolverCreato~",
                        column: x => x.ResolverCreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_series",
                columns: table => new
                {
                    SeriesId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_series", x => new { x.SeriesId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_artwork_series_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_series_penomy_series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "penomy_series",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_violation_flag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ViolationFlagTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveStatus = table.Column<int>(type: "integer", nullable: false),
                    ResolveNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_violation_flag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_penomy_artwork_violation_flag~",
                        column: x => x.ViolationFlagTypeId,
                        principalTable: "penomy_artwork_violation_flag_type",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_penomy_creator_profile_Resolv~",
                        column: x => x.ResolvedBy,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_violation_flag_penomy_system_account_Created~",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_system_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_creator_collaborated_artwork",
                columns: table => new
                {
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    GrantedBy = table.Column<long>(type: "bigint", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_creator_collaborated_artwork", x => new { x.CreatorId, x.ArtworkId, x.RoleId, x.GrantedBy });
                    table.ForeignKey(
                        name: "FK_penomy_creator_collaborated_artwork_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_creator_collaborated_artwork_penomy_creator_profile_~",
                        column: x => x.CreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_creator_collaborated_artwork_penomy_creator_profile~1",
                        column: x => x.GrantedBy,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_social_group_related_artwork",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_social_group_related_artwork", x => new { x.GroupId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_social_group_related_artwork_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_social_group_related_artwork_penomy_social_group_Gro~",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_favorite_artwork",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkType = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_favorite_artwork", x => new { x.UserId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_user_favorite_artwork_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_followed_artwork",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkType = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_followed_artwork", x => new { x.UserId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_user_followed_artwork_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_rating_artwork",
                columns: table => new
                {
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StarRates = table.Column<byte>(type: "smallint", nullable: false),
                    RatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_rating_artwork", x => new { x.UserId, x.ArtworkId });
                    table.ForeignKey(
                        name: "FK_penomy_user_rating_artwork_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_bug_report_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BugReportId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_bug_report_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_bug_report_attached_media_penomy_bug_report_BugRepor~",
                        column: x => x.BugReportId,
                        principalTable: "penomy_bug_report",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_message_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ChatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_message_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_chat_message_attached_media_penomy_chat_message_Chat~",
                        column: x => x.ChatMessageId,
                        principalTable: "penomy_chat_message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_chat_message_like_statistic",
                columns: table => new
                {
                    ChatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_chat_message_like_statistic", x => new { x.ChatMessageId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_penomy_chat_message_like_statistic_penomy_chat_message_Chat~",
                        column: x => x.ChatMessageId,
                        principalTable: "penomy_chat_message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_chat_message_like_statistic_penomy_user_like_value_L~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_chat_message",
                columns: table => new
                {
                    ChatMessageId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_chat_message", x => new { x.ChatMessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_penomy_user_like_chat_message_penomy_chat_message_ChatMessa~",
                        column: x => x.ChatMessageId,
                        principalTable: "penomy_chat_message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_chat_message_penomy_user_like_value_LikeVa~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_chat_message_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_creator_wallet_transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    WalletId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TransactionStatus = table.Column<int>(type: "integer", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TransactionMetaData = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_creator_wallet_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_creator_wallet_transaction_penomy_creator_wallet_Wal~",
                        column: x => x.WalletId,
                        principalTable: "penomy_creator_wallet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_creator_wallet_transaction_penomy_creator_wallet_tra~",
                        column: x => x.TransactionTypeId,
                        principalTable: "penomy_creator_wallet_transaction_type",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_pinned_post",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    PinnedBy = table.Column<long>(type: "bigint", nullable: false),
                    PinnedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_pinned_post", x => new { x.GroupId, x.PostId, x.PinnedBy });
                    table.ForeignKey(
                        name: "FK_penomy_group_pinned_post_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_pinned_post_penomy_social_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "penomy_social_group",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_pinned_post_penomy_user_profile_PinnedBy",
                        column: x => x.PinnedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_attached_media_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_comment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    TotalChildComments = table.Column<int>(type: "integer", nullable: false),
                    IsDirectlyCommented = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_like_statistic",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_like_statistic", x => new { x.PostId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_penomy_group_post_like_statistic_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_like_statistic_penomy_user_like_value_Pos~",
                        column: x => x.PostId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_report_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_report_penomy_post_report_problem_ReportP~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_post_report_problem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_group_post",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_group_post", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_penomy_user_like_value_LikeValu~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_saved_group_post",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_saved_group_post", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_penomy_user_saved_group_post_penomy_group_post_PostId",
                        column: x => x.PostId,
                        principalTable: "penomy_group_post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_saved_group_post_penomy_user_profile_UserId",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_user_post_comment",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_user_post_comment", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_comment_penomy_user_like_value_L~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_comment_penomy_user_post_comment~",
                        column: x => x.CommentId,
                        principalTable: "penomy_user_post_comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_user_post_comment_penomy_user_profile_User~",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_comment_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_comment_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_attached_media_penomy_user_post_co~",
                        column: x => x.CommentId,
                        principalTable: "penomy_user_post_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_comment_like_statistics",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_comment_like_statistics", x => new { x.CommentId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_like_statistics_penomy_user_like_v~",
                        column: x => x.CommentId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_like_statistics_penomy_user_post_c~",
                        column: x => x.CommentId,
                        principalTable: "penomy_user_post_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_post_comment_parent_child",
                columns: table => new
                {
                    ParentCommentId = table.Column<long>(type: "bigint", nullable: false),
                    ChildCommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_post_comment_parent_child", x => new { x.ParentCommentId, x.ChildCommentId });
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_parent_child_penomy_user_post_comm~",
                        column: x => x.ChildCommentId,
                        principalTable: "penomy_user_post_comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_user_post_comment_parent_child_penomy_user_post_com~1",
                        column: x => x.ParentCommentId,
                        principalTable: "penomy_user_post_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_bug_report_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BugReportId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_bug_report_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_bug_report_attached_media_penomy_artwork_bug~",
                        column: x => x.BugReportId,
                        principalTable: "penomy_artwork_bug_report",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_chapter_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_chapter_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_media_penomy_artwork_chapter_Chapter~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_chapter_metadata",
                columns: table => new
                {
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    TotalViews = table.Column<long>(type: "bigint", nullable: false),
                    TotalFavorites = table.Column<long>(type: "bigint", nullable: false),
                    TotalComments = table.Column<long>(type: "bigint", nullable: false),
                    HasAdRevenueEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_chapter_metadata", x => x.ChapterId);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_metadata_penomy_artwork_chapter_Chap~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_chapter_report",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    ReportProblemId = table.Column<long>(type: "bigint", nullable: false),
                    DetailNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolveStatus = table.Column<int>(type: "integer", nullable: false),
                    ResolveNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ResolvedBy = table.Column<long>(type: "bigint", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ResolverCreatorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_chapter_report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_penomy_artwork_chapter_Chapte~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_penomy_artwork_report_problem~",
                        column: x => x.ReportProblemId,
                        principalTable: "penomy_artwork_report_problem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_penomy_creator_profile_Resolv~",
                        column: x => x.ResolverCreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_penomy_user_profile_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_artwork_view_history",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkType = table.Column<int>(type: "integer", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_artwork_view_history", x => new { x.UserId, x.ArtworkId, x.ChapterId });
                    table.ForeignKey(
                        name: "FK_penomy_user_artwork_view_history_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_artwork_view_history_penomy_artwork_chapter_Cha~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_watching_history",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<long>(type: "bigint", nullable: false),
                    ChapterId = table.Column<long>(type: "bigint", nullable: false),
                    WatchedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_watching_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_watching_history_penomy_artwork_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "penomy_artwork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_user_watching_history_penomy_artwork_chapter_Chapter~",
                        column: x => x.ChapterId,
                        principalTable: "penomy_artwork_chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_report_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkReportedId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_report_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_report_attached_media_penomy_artwork_report_~",
                        column: x => x.ArtworkReportedId,
                        principalTable: "penomy_artwork_report",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_donation_transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    DonatorId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    WalletTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TransactionStatus = table.Column<int>(type: "integer", nullable: false),
                    TotalDonationAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatorReceivedPercentage = table.Column<int>(type: "integer", nullable: false),
                    HasReceivedThankFromCreator = table.Column<bool>(type: "boolean", nullable: false),
                    DonationNote = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_donation_transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_user_donation_transaction_penomy_creator_profile_Cre~",
                        column: x => x.CreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_user_donation_transaction_penomy_creator_wallet_tran~",
                        column: x => x.WalletTransactionId,
                        principalTable: "penomy_creator_wallet_transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_user_donation_transaction_penomy_user_profile_Donato~",
                        column: x => x.DonatorId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_comment_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_comment_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_attached_media_penomy_group_post_~",
                        column: x => x.CommentId,
                        principalTable: "penomy_group_post_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_comment_like_statistic",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    Total = table.Column<long>(type: "bigint", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_comment_like_statistic", x => new { x.CommentId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_like_statistic_penomy_group_post_~",
                        column: x => x.CommentId,
                        principalTable: "penomy_group_post_comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_like_statistic_penomy_user_like_v~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_group_post_comment_parent_child",
                columns: table => new
                {
                    ParentCommentId = table.Column<long>(type: "bigint", nullable: false),
                    ChildCommentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_group_post_comment_parent_child", x => new { x.ParentCommentId, x.ChildCommentId });
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_parent_child_penomy_group_post_co~",
                        column: x => x.ChildCommentId,
                        principalTable: "penomy_group_post_comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_penomy_group_post_comment_parent_child_penomy_group_post_c~1",
                        column: x => x.ParentCommentId,
                        principalTable: "penomy_group_post_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_like_group_post_comment",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ValueId = table.Column<long>(type: "bigint", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    LikeValueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_like_group_post_comment", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_comment_penomy_group_post_comme~",
                        column: x => x.CommentId,
                        principalTable: "penomy_group_post_comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_comment_penomy_user_like_value_~",
                        column: x => x.LikeValueId,
                        principalTable: "penomy_user_like_value",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_like_group_post_comment_penomy_user_profile_Use~",
                        column: x => x.UserId,
                        principalTable: "penomy_user_profile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "penomy_artwork_chapter_report_attached_media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ChapterReportedId = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    UploadOrder = table.Column<int>(type: "integer", nullable: false),
                    StorageUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_artwork_chapter_report_attached_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_penomy_artwork_chapter_report_attached_media_penomy_artwork~",
                        column: x => x.ChapterReportedId,
                        principalTable: "penomy_artwork_chapter_report",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_donation_thank",
                columns: table => new
                {
                    UserDonationTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    ThankNote = table.Column<string>(type: "character varying(640)", maxLength: 640, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_donation_thank", x => new { x.UserDonationTransactionId, x.CreatorId });
                    table.ForeignKey(
                        name: "FK_penomy_donation_thank_penomy_creator_profile_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "penomy_creator_profile",
                        principalColumn: "CreatorId");
                    table.ForeignKey(
                        name: "FK_penomy_donation_thank_penomy_user_donation_transaction_User~",
                        column: x => x.UserDonationTransactionId,
                        principalTable: "penomy_user_donation_transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "penomy_user_donation_transaction_item",
                columns: table => new
                {
                    DonationTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    DonationItemId = table.Column<long>(type: "bigint", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ItemQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_penomy_user_donation_transaction_item", x => new { x.DonationTransactionId, x.DonationItemId });
                    table.ForeignKey(
                        name: "FK_penomy_user_donation_transaction_item_penomy_donation_item_~",
                        column: x => x.DonationItemId,
                        principalTable: "penomy_donation_item",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_penomy_user_donation_transaction_item_penomy_user_donation_~",
                        column: x => x.DonationTransactionId,
                        principalTable: "penomy_user_donation_transaction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_ArtworkOriginId",
                table: "penomy_artwork",
                column: "ArtworkOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_CreatedBy",
                table: "penomy_artwork",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_TemporarilyRemovedBy",
                table: "penomy_artwork",
                column: "TemporarilyRemovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_UpdatedBy",
                table: "penomy_artwork",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_applied_revenue_program_ApprovedBy",
                table: "penomy_artwork_applied_revenue_program",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_applied_revenue_program_ProposedBy",
                table: "penomy_artwork_applied_revenue_program",
                column: "ProposedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_report_ArtworkId",
                table: "penomy_artwork_bug_report",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_report_BugTypeId",
                table: "penomy_artwork_bug_report",
                column: "BugTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_report_CreatedBy",
                table: "penomy_artwork_bug_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_report_ResolvedBy",
                table: "penomy_artwork_bug_report",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_report_attached_media_BugReportId",
                table: "penomy_artwork_bug_report_attached_media",
                column: "BugReportId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_bug_type_CreatedBy",
                table: "penomy_artwork_bug_type",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_category_CategoryId",
                table: "penomy_artwork_category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_ArtworkId",
                table: "penomy_artwork_chapter",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_CreatedBy",
                table: "penomy_artwork_chapter",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_TemporarilyRemovedBy",
                table: "penomy_artwork_chapter",
                column: "TemporarilyRemovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_UpdatedBy",
                table: "penomy_artwork_chapter",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_media_ChapterId",
                table: "penomy_artwork_chapter_media",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_ArtworkId",
                table: "penomy_artwork_chapter_report",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_ChapterId",
                table: "penomy_artwork_chapter_report",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_CreatedBy",
                table: "penomy_artwork_chapter_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_ReportProblemId",
                table: "penomy_artwork_chapter_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_ResolverCreatorId",
                table: "penomy_artwork_chapter_report",
                column: "ResolverCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_chapter_report_attached_media_ChapterReporte~",
                table: "penomy_artwork_chapter_report_attached_media",
                column: "ChapterReportedId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_comment_CreatedBy",
                table: "penomy_artwork_comment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_comment_parent_child_ChildCommentId",
                table: "penomy_artwork_comment_parent_child",
                column: "ChildCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_comment_reference_ArtworkId",
                table: "penomy_artwork_comment_reference",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_origin_CreatedBy",
                table: "penomy_artwork_origin",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_origin_UpdatedBy",
                table: "penomy_artwork_origin",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_other_info_OtherInfoId",
                table: "penomy_artwork_other_info",
                column: "OtherInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_ArtworkId",
                table: "penomy_artwork_report",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_CreatedBy",
                table: "penomy_artwork_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_ReportProblemId",
                table: "penomy_artwork_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_ResolverCreatorId",
                table: "penomy_artwork_report",
                column: "ResolverCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_attached_media_ArtworkReportedId",
                table: "penomy_artwork_report_attached_media",
                column: "ArtworkReportedId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_report_problem_CreatedBy",
                table: "penomy_artwork_report_problem",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_series_ArtworkId",
                table: "penomy_artwork_series",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_ArtworkId",
                table: "penomy_artwork_violation_flag",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_CreatedBy",
                table: "penomy_artwork_violation_flag",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_ResolvedBy",
                table: "penomy_artwork_violation_flag",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_ViolationFlagTypeId",
                table: "penomy_artwork_violation_flag",
                column: "ViolationFlagTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_type_CreatedBy",
                table: "penomy_artwork_violation_flag_type",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_artwork_violation_flag_type_UpdatedBy",
                table: "penomy_artwork_violation_flag_type",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_ban_type_CreatedBy",
                table: "penomy_ban_type",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bank_CreatedBy",
                table: "penomy_bank",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bank_UpdatedBy",
                table: "penomy_bank",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bug_report_BugTypeId",
                table: "penomy_bug_report",
                column: "BugTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bug_report_CreatedBy",
                table: "penomy_bug_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bug_report_ResolvedBy",
                table: "penomy_bug_report",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bug_report_attached_media_BugReportId",
                table: "penomy_bug_report_attached_media",
                column: "BugReportId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_bug_type_CreatedBy",
                table: "penomy_bug_type",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_category_CreatedBy",
                table: "penomy_category",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_category_UpdatedBy",
                table: "penomy_category",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_group_CreatedBy",
                table: "penomy_chat_group",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_group_join_request_CreatedBy",
                table: "penomy_chat_group_join_request",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_group_member_ChatGroupId",
                table: "penomy_chat_group_member",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_message_ChatGroupId",
                table: "penomy_chat_message",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_message_CreatedBy",
                table: "penomy_chat_message",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_message_attached_media_ChatMessageId",
                table: "penomy_chat_message_attached_media",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_chat_message_like_statistic_LikeValueId",
                table: "penomy_chat_message_like_statistic",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_collaborated_artwork_ArtworkId",
                table: "penomy_creator_collaborated_artwork",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_collaborated_artwork_GrantedBy",
                table: "penomy_creator_collaborated_artwork",
                column: "GrantedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_wallet_BankId",
                table: "penomy_creator_wallet",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_wallet_CreatorId",
                table: "penomy_creator_wallet",
                column: "CreatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_wallet_transaction_TransactionTypeId",
                table: "penomy_creator_wallet_transaction",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_wallet_transaction_WalletId",
                table: "penomy_creator_wallet_transaction",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_creator_wallet_transaction_type_CreatorId",
                table: "penomy_creator_wallet_transaction_type",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_donation_item_CreatedBy",
                table: "penomy_donation_item",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_donation_item_UpdatedBy",
                table: "penomy_donation_item",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_donation_thank_CreatorId",
                table: "penomy_donation_thank",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_donation_thank_UserDonationTransactionId",
                table: "penomy_donation_thank",
                column: "UserDonationTransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_granted_authorized_user_AuthorizedUserId",
                table: "penomy_granted_authorized_user",
                column: "AuthorizedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_granted_authorized_user_GrantedBy",
                table: "penomy_granted_authorized_user",
                column: "GrantedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_pinned_post_PinnedBy",
                table: "penomy_group_pinned_post",
                column: "PinnedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_pinned_post_PostId",
                table: "penomy_group_pinned_post",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_ApprovedBy",
                table: "penomy_group_post",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_CreatedBy",
                table: "penomy_group_post",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_GroupId",
                table: "penomy_group_post",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_attached_media_PostId",
                table: "penomy_group_post_attached_media",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_comment_CreatedBy",
                table: "penomy_group_post_comment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_comment_PostId",
                table: "penomy_group_post_comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_comment_attached_media_CommentId",
                table: "penomy_group_post_comment_attached_media",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_comment_like_statistic_LikeValueId",
                table: "penomy_group_post_comment_like_statistic",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_comment_parent_child_ChildCommentId",
                table: "penomy_group_post_comment_parent_child",
                column: "ChildCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_report_CreatedBy",
                table: "penomy_group_post_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_report_PostId",
                table: "penomy_group_post_report",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_group_post_report_ReportProblemId",
                table: "penomy_group_post_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "penomy_identity_role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_identity_roleclaim_RoleId",
                table: "penomy_identity_roleclaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "penomy_identity_user",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "penomy_identity_user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_identity_userclaim_UserId",
                table: "penomy_identity_userclaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_identity_userlogin_UserId",
                table: "penomy_identity_userlogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_identity_userrole_RoleId",
                table: "penomy_identity_userrole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_mail_server_status_MailDomain",
                table: "penomy_mail_server_status",
                column: "MailDomain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_other_info_CreatedBy",
                table: "penomy_other_info",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_post_report_problem_CreatedBy",
                table: "penomy_post_report_problem",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_register_waiting_list_Email",
                table: "penomy_register_waiting_list",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_revenue_program_CreatedBy",
                table: "penomy_revenue_program",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_revenue_program_UpdatedBy",
                table: "penomy_revenue_program",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_series_CreatedBy",
                table: "penomy_series",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_series_TemporarilyRemovedBy",
                table: "penomy_series",
                column: "TemporarilyRemovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_series_UpdatedBy",
                table: "penomy_series",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_CreatedBy",
                table: "penomy_social_group",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_join_request_CreatedBy",
                table: "penomy_social_group_join_request",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_linked_chat_group_ChatGroupId",
                table: "penomy_social_group_linked_chat_group",
                column: "ChatGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_linked_chat_group_SocialGroupId",
                table: "penomy_social_group_linked_chat_group",
                column: "SocialGroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_member_MemberId",
                table: "penomy_social_group_member",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_related_artwork_ArtworkId",
                table: "penomy_social_group_related_artwork",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_report_CreatedBy",
                table: "penomy_social_group_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_report_GroupId",
                table: "penomy_social_group_report",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_report_ReportProblemId",
                table: "penomy_social_group_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_report_problem_CreatedBy",
                table: "penomy_social_group_report_problem",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_violation_flag_CreatedBy",
                table: "penomy_social_group_violation_flag",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_violation_flag_GroupId",
                table: "penomy_social_group_violation_flag",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_social_group_violation_flag_ResolvedBy",
                table: "penomy_social_group_violation_flag",
                column: "ResolvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_system_account_role_RoleId",
                table: "penomy_system_account_role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_artwork_view_history_ArtworkId",
                table: "penomy_user_artwork_view_history",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_artwork_view_history_ChapterId",
                table: "penomy_user_artwork_view_history",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_ban_BannedBy",
                table: "penomy_user_ban",
                column: "BannedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_ban_BanTypeId",
                table: "penomy_user_ban",
                column: "BanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_ban_UserId",
                table: "penomy_user_ban",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_chat_group_active_history_ChatGroupId",
                table: "penomy_user_chat_group_active_history",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_donation_transaction_CreatorId",
                table: "penomy_user_donation_transaction",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_donation_transaction_DonatorId",
                table: "penomy_user_donation_transaction",
                column: "DonatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_donation_transaction_WalletTransactionId",
                table: "penomy_user_donation_transaction",
                column: "WalletTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_donation_transaction_item_DonationItemId",
                table: "penomy_user_donation_transaction_item",
                column: "DonationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_favorite_artwork_ArtworkId",
                table: "penomy_user_favorite_artwork",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_followed_artwork_ArtworkId",
                table: "penomy_user_followed_artwork",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_followed_creator_CreatorId",
                table: "penomy_user_followed_creator",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_chat_message_LikeValueId",
                table: "penomy_user_like_chat_message",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_chat_message_UserId",
                table: "penomy_user_like_chat_message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_LikeValueId",
                table: "penomy_user_like_group_post",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_UserId",
                table: "penomy_user_like_group_post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_comment_LikeValueId",
                table: "penomy_user_like_group_post_comment",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_group_post_comment_UserId",
                table: "penomy_user_like_group_post_comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_LikeValueId",
                table: "penomy_user_like_user_post",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_UserId",
                table: "penomy_user_like_user_post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_comment_LikeValueId",
                table: "penomy_user_like_user_post_comment",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_user_post_comment_UserId",
                table: "penomy_user_like_user_post_comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_like_value_CreatedBy",
                table: "penomy_user_like_value",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_CreatedBy",
                table: "penomy_user_post",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_attached_media_PostId",
                table: "penomy_user_post_attached_media",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_comment_CreatedBy",
                table: "penomy_user_post_comment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_comment_PostId",
                table: "penomy_user_post_comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_comment_attached_media_CommentId",
                table: "penomy_user_post_comment_attached_media",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_comment_parent_child_ChildCommentId",
                table: "penomy_user_post_comment_parent_child",
                column: "ChildCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_like_statistics_LikeValueId",
                table: "penomy_user_post_like_statistics",
                column: "LikeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_report_CreatedBy",
                table: "penomy_user_post_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_report_PostId",
                table: "penomy_user_post_report",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_post_report_ReportProblemId",
                table: "penomy_user_post_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_profile_report_CreatedBy",
                table: "penomy_user_profile_report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_profile_report_ReportedProfileId",
                table: "penomy_user_profile_report",
                column: "ReportedProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_profile_report_ReportProblemId",
                table: "penomy_user_profile_report",
                column: "ReportProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_profile_report_problem_CreatedBy",
                table: "penomy_user_profile_report_problem",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_rating_artwork_ArtworkId",
                table: "penomy_user_rating_artwork",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_saved_group_post_PostId",
                table: "penomy_user_saved_group_post",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_saved_user_posts_PostId",
                table: "penomy_user_saved_user_posts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_watching_history_ArtworkId",
                table: "penomy_user_watching_history",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_penomy_user_watching_history_ChapterId",
                table: "penomy_user_watching_history",
                column: "ChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "penomy_artwork_applied_revenue_program");

            migrationBuilder.DropTable(
                name: "penomy_artwork_bug_report_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_artwork_category");

            migrationBuilder.DropTable(
                name: "penomy_artwork_chapter_media");

            migrationBuilder.DropTable(
                name: "penomy_artwork_chapter_metadata");

            migrationBuilder.DropTable(
                name: "penomy_artwork_chapter_report_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_artwork_comment_parent_child");

            migrationBuilder.DropTable(
                name: "penomy_artwork_comment_reference");

            migrationBuilder.DropTable(
                name: "penomy_artwork_metadata");

            migrationBuilder.DropTable(
                name: "penomy_artwork_other_info");

            migrationBuilder.DropTable(
                name: "penomy_artwork_report_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_artwork_series");

            migrationBuilder.DropTable(
                name: "penomy_artwork_violation_flag");

            migrationBuilder.DropTable(
                name: "penomy_bug_report_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_chat_group_join_request");

            migrationBuilder.DropTable(
                name: "penomy_chat_group_member");

            migrationBuilder.DropTable(
                name: "penomy_chat_message_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_chat_message_like_statistic");

            migrationBuilder.DropTable(
                name: "penomy_chat_message_reply");

            migrationBuilder.DropTable(
                name: "penomy_creator_collaborated_artwork");

            migrationBuilder.DropTable(
                name: "penomy_donation_thank");

            migrationBuilder.DropTable(
                name: "penomy_granted_authorized_user");

            migrationBuilder.DropTable(
                name: "penomy_group_pinned_post");

            migrationBuilder.DropTable(
                name: "penomy_group_post_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_group_post_comment_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_group_post_comment_like_statistic");

            migrationBuilder.DropTable(
                name: "penomy_group_post_comment_parent_child");

            migrationBuilder.DropTable(
                name: "penomy_group_post_like_statistic");

            migrationBuilder.DropTable(
                name: "penomy_group_post_report");

            migrationBuilder.DropTable(
                name: "penomy_identity_roleclaim");

            migrationBuilder.DropTable(
                name: "penomy_identity_userclaim");

            migrationBuilder.DropTable(
                name: "penomy_identity_userlogin");

            migrationBuilder.DropTable(
                name: "penomy_identity_userrole");

            migrationBuilder.DropTable(
                name: "penomy_identity_usertoken");

            migrationBuilder.DropTable(
                name: "penomy_mail_server_status");

            migrationBuilder.DropTable(
                name: "penomy_register_waiting_list");

            migrationBuilder.DropTable(
                name: "penomy_revenue_program");

            migrationBuilder.DropTable(
                name: "penomy_social_group_join_request");

            migrationBuilder.DropTable(
                name: "penomy_social_group_linked_chat_group");

            migrationBuilder.DropTable(
                name: "penomy_social_group_member");

            migrationBuilder.DropTable(
                name: "penomy_social_group_related_artwork");

            migrationBuilder.DropTable(
                name: "penomy_social_group_report");

            migrationBuilder.DropTable(
                name: "penomy_social_group_violation_flag");

            migrationBuilder.DropTable(
                name: "penomy_system_account_role");

            migrationBuilder.DropTable(
                name: "penomy_user_artwork_view_history");

            migrationBuilder.DropTable(
                name: "penomy_user_ban");

            migrationBuilder.DropTable(
                name: "penomy_user_chat_group_active_history");

            migrationBuilder.DropTable(
                name: "penomy_user_donation_transaction_item");

            migrationBuilder.DropTable(
                name: "penomy_user_favorite_artwork");

            migrationBuilder.DropTable(
                name: "penomy_user_followed_artwork");

            migrationBuilder.DropTable(
                name: "penomy_user_followed_creator");

            migrationBuilder.DropTable(
                name: "penomy_user_friend");

            migrationBuilder.DropTable(
                name: "penomy_user_friend_request");

            migrationBuilder.DropTable(
                name: "penomy_user_like_artwork_comment");

            migrationBuilder.DropTable(
                name: "penomy_user_like_chat_message");

            migrationBuilder.DropTable(
                name: "penomy_user_like_group_post");

            migrationBuilder.DropTable(
                name: "penomy_user_like_group_post_comment");

            migrationBuilder.DropTable(
                name: "penomy_user_like_user_post");

            migrationBuilder.DropTable(
                name: "penomy_user_like_user_post_comment");

            migrationBuilder.DropTable(
                name: "penomy_user_post_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_user_post_comment_attached_media");

            migrationBuilder.DropTable(
                name: "penomy_user_post_comment_like_statistics");

            migrationBuilder.DropTable(
                name: "penomy_user_post_comment_parent_child");

            migrationBuilder.DropTable(
                name: "penomy_user_post_like_statistics");

            migrationBuilder.DropTable(
                name: "penomy_user_post_report");

            migrationBuilder.DropTable(
                name: "penomy_user_profile_report");

            migrationBuilder.DropTable(
                name: "penomy_user_rating_artwork");

            migrationBuilder.DropTable(
                name: "penomy_user_saved_group_post");

            migrationBuilder.DropTable(
                name: "penomy_user_saved_user_posts");

            migrationBuilder.DropTable(
                name: "penomy_user_watching_history");

            migrationBuilder.DropTable(
                name: "penomy_artwork_bug_report");

            migrationBuilder.DropTable(
                name: "penomy_category");

            migrationBuilder.DropTable(
                name: "penomy_artwork_chapter_report");

            migrationBuilder.DropTable(
                name: "penomy_artwork_comment");

            migrationBuilder.DropTable(
                name: "penomy_other_info");

            migrationBuilder.DropTable(
                name: "penomy_artwork_report");

            migrationBuilder.DropTable(
                name: "penomy_series");

            migrationBuilder.DropTable(
                name: "penomy_artwork_violation_flag_type");

            migrationBuilder.DropTable(
                name: "penomy_bug_report");

            migrationBuilder.DropTable(
                name: "penomy_identity_role");

            migrationBuilder.DropTable(
                name: "penomy_identity_user");

            migrationBuilder.DropTable(
                name: "penomy_social_group_report_problem");

            migrationBuilder.DropTable(
                name: "penomy_system_role");

            migrationBuilder.DropTable(
                name: "penomy_ban_type");

            migrationBuilder.DropTable(
                name: "penomy_donation_item");

            migrationBuilder.DropTable(
                name: "penomy_user_donation_transaction");

            migrationBuilder.DropTable(
                name: "penomy_chat_message");

            migrationBuilder.DropTable(
                name: "penomy_group_post_comment");

            migrationBuilder.DropTable(
                name: "penomy_user_post_comment");

            migrationBuilder.DropTable(
                name: "penomy_user_like_value");

            migrationBuilder.DropTable(
                name: "penomy_post_report_problem");

            migrationBuilder.DropTable(
                name: "penomy_user_profile_report_problem");

            migrationBuilder.DropTable(
                name: "penomy_artwork_bug_type");

            migrationBuilder.DropTable(
                name: "penomy_artwork_chapter");

            migrationBuilder.DropTable(
                name: "penomy_artwork_report_problem");

            migrationBuilder.DropTable(
                name: "penomy_bug_type");

            migrationBuilder.DropTable(
                name: "penomy_creator_wallet_transaction");

            migrationBuilder.DropTable(
                name: "penomy_chat_group");

            migrationBuilder.DropTable(
                name: "penomy_group_post");

            migrationBuilder.DropTable(
                name: "penomy_user_post");

            migrationBuilder.DropTable(
                name: "penomy_artwork");

            migrationBuilder.DropTable(
                name: "penomy_creator_wallet");

            migrationBuilder.DropTable(
                name: "penomy_creator_wallet_transaction_type");

            migrationBuilder.DropTable(
                name: "penomy_social_group");

            migrationBuilder.DropTable(
                name: "penomy_artwork_origin");

            migrationBuilder.DropTable(
                name: "penomy_bank");

            migrationBuilder.DropTable(
                name: "penomy_creator_profile");

            migrationBuilder.DropTable(
                name: "penomy_system_account");

            migrationBuilder.DropTable(
                name: "penomy_user_profile");
        }
    }
}
