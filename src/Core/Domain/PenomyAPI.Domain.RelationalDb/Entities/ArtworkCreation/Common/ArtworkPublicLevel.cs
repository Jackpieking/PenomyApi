namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

/// <summary>
///     This enum is apply for both <see cref="Artwork"/> and <see cref="ArtworkChapter"/>.
/// </summary>
public enum ArtworkPublicLevel
{
    Everyone = 1,

    OnlyFriend = 2,

    Private = 3,

    PrivateWithLimitedUsers = 4
}
