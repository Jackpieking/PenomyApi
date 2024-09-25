namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

/// <summary>
///     This public level will restrict the people
///     who could see the user post.
/// </summary>
public enum UserPostPublicLevel
{
    Everyone = 1,

    OnlyFriend = 2,

    Private = 3,

    PrivateWithLimitedUsers = 4
}
