using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class Artwork
    : EntityWithId<long>,
        ICreatedEntity<long>,
        IUpdatedEntity<long>,
        ITemporarilyRemovedEntity<long>
{
    public string Title { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public string AuthorName { get; set; }

    public bool HasSeries { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    /// <summary>
    ///     The upload order of the latest chapter
    ///     that belonged to this artwork
    /// </summary>
    public int LastChapterUploadOrder { get; set; }

    /// <summary>
    ///     This property is supported for artwork type animation.
    /// </summary>
    public int FixedTotalChapters { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public ArtworkType ArtworkType { get; set; }

    public long ArtworkOriginId { get; set; }

    /// <summary>
    ///     Allow the users to comment at the
    ///     detail page of this artwork.
    /// </summary>
    public bool AllowComment { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     This field is used to check if this artwork
    ///     is uploaded by the authorized user or not.
    /// </summary>
    public bool IsCreatedByAuthorizedUser { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsTemporarilyRemoved { get; set; }

    public long TemporarilyRemovedBy { get; set; }

    public DateTime TemporarilyRemovedAt { get; set; }

    /// <summary>
    ///     Only the artwork that violated the platform's
    ///     content policy will have this <see cref="IsTakenDown"/>
    ///     property equal <see langword="true"/>.
    /// </summary>
    /// <remarks>
    ///     Only system account with role equal Artwork Manager
    ///     can set the value for this property.
    /// </remarks>
    public bool IsTakenDown { get; set; } = false;

    #region Navigation
    public ArtworkOrigin Origin { get; set; }

    public ArtworkMetaData ArtworkMetaData { get; set; }

    public UserProfile Creator { get; set; }

    public UserProfile Updater { get; set; }

    public UserProfile Remover { get; set; }

    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    public IEnumerable<ArtworkChapter> Chapters { get; set; }

    public IEnumerable<ArtworkSeries> ArtworkSeries { get; set; }

    public IEnumerable<UserFollowedArtwork> UserFollowedArtworks { get; set; }

    public IEnumerable<UserFavoriteArtwork> UserFavoriteArtworks { get; set; }

    public IEnumerable<UserRatingArtwork> UserRatingArtworks { get; set; }

    public IEnumerable<UserArtworkViewHistory> UserArtworkViewHistories { get; set; }

    public IEnumerable<UserWatchingHistory> UserWatchingHistories { get; set; }

    public IEnumerable<CreatorCollaboratedArtwork> UserManagedArtworks { get; set; }

    public IEnumerable<ArtworkCommentReference> CommentReferences { get; set; }

    public IEnumerable<ArtworkOtherInfo> ArtworkOtherInfos { get; set; }

    // Report and Violation section
    public IEnumerable<ArtworkReport> ArtworkReports { get; set; }

    public IEnumerable<ArtworkChapterReport> ArtworkChapterReports { get; set; }

    public IEnumerable<ArtworkBugReport> ArtworkBugReports { get; set; }

    public IEnumerable<ArtworkViolationFlag> ViolationFlags { get; set; }

    // Social Media section
    public IEnumerable<SocialGroupRelatedArtwork> SocialGroupRelatedArtworks { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int OtherNameLength = 200;

        public const int StudioNameLength = 200;

        public const int AuthorNameLength = 100;

        public const int ThumbnailUrlLength = 2000;

        public const int IntroductionLength = 2000;
    }
    #endregion
}
