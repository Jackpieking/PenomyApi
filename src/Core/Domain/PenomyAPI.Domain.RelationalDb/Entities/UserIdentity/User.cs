using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class User
{
    public long Id { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public int AccessFailedCount { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string SecurityStamp { get; set; }

    public DateTimeOffset LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public static class MetaData
    {
        public const int UserNameLength = 256;

        // Reference from: https://stackoverflow.com/questions/1199190/what-is-the-optimal-length-for-an-email-address-in-a-database
        public const int EmailLength = 256;

        public const int PasswordHashLength = 256;

        // Reference from: https://stackoverflow.com/questions/723587/whats-the-longest-possible-worldwide-phone-number-i-should-consider-in-sql-varc
        public const int PhoneNumberLength = 15;

        public const int ConcurrencyStampLength = 64;

        public const int SecurityStampLength = ConcurrencyStampLength;
    }
}
