namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public enum SocialGroupStatus
{
    /// <summary>
    ///     This status will be set as a default when a social group is created.
    /// </summary>
    Active = 1,

    /// <summary>
    ///     This status will be set when the social group is disband by group manager.
    ///     Any group has this status will be automatically processed and deleted by the system.
    /// </summary>
    Disband = 2,

    /// <summary>
    ///     This status will be set when the social group violates platform policy
    ///     and taken down by the social media manager.
    /// </summary>
    TakenDown = 3,
}
