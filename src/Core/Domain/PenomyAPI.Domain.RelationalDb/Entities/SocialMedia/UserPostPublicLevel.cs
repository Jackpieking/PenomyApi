namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

/// <summary>
///     This public level will restrict the people
///     who could see the user post.
/// </summary>
public enum UserPostPublicLevel
{
    OnlyMe = 1,

    OnlyFriends = 2,

    Everyone = 3
}
