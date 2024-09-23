using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.Subscriptions;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

public sealed class SystemAccount : EntityWithId<long>
{
    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    /// <summary>
    ///     Check if the current system is active or not.
    /// </summary>
    /// <remarks>
    ///     true: This system account is available to sign in. <br/>
    ///     false: This system account is inactive and cannot access.
    ///     The system account with this state was disabled by the super admin.
    /// </remarks>
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    // Subscription section.
    /// <summary>
    ///     Contains all subscription plan this system account created.
    /// </summary>
    public IEnumerable<SubscriptionPlan> CreatedSubscriptionPlans { get; set; }

    public IEnumerable<SystemAccountRole> SystemAccountRoles { get; set; }

    // Generic section.
    public IEnumerable<UserBan> UserBans { get; set; }

    public IEnumerable<BanType> CreatedBanTypes { get; set; }

    public IEnumerable<BugType> CreatedBugTypes { get; set; }

    public IEnumerable<UserReportProblem> CreatedUserReportProblems { get; set; }

    public IEnumerable<GrantedAuthorizedUser> GrantedAuthorizedUsers { get; set; }

    public IEnumerable<GrantedAuthorizedUser> ReceivedAuthorizedUsers { get; set; }

    public IEnumerable<BugReport> ResolvedBugReports { get; set; }

    // Social media section.
    public IEnumerable<PostReportProblem> CreatedPostReportProblems { get; set; }

    // Artwork section.
    public IEnumerable<Category> CreatedCategories { get; set; }

    public IEnumerable<Category> UpdatedCategories { get; set; }

    public IEnumerable<ArtworkOrigin> CreatedOrigins { get; set; }

    public IEnumerable<ArtworkOrigin> UpdatedOrigins { get; set; }

    public IEnumerable<ArtworkReportProblem> CreatedArtworkReportProblems { get; set; }

    public IEnumerable<ArtworkViolationFlag> CreatedArtworkViolationFlags { get; set; }

    public IEnumerable<ArtworkViolationFlagType> CreatedArtworkViolationFlagTypes { get; set; }

    public IEnumerable<ArtworkBugReport> ResolvedArtworkBugReports { get; set; }

    public IEnumerable<ArtworkBugType> CreatedArtworkBugTypes { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        // Reference from: https://stackoverflow.com/questions/1199190/what-is-the-optimal-length-for-an-email-address-in-a-database
        public const int EmailLength = 256;

        public const int PasswordHashLength = 256;

        // Reference from: https://stackoverflow.com/questions/723587/whats-the-longest-possible-worldwide-phone-number-i-should-consider-in-sql-varc
        public const int PhoneNumberLength = 15;
    }
    #endregion
}
