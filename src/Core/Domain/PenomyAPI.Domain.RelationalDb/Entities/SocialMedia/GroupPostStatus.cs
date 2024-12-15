namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

/// <summary>
///     The status of the group post when created in group.
///     Please reference the 02_ERD_Description (supported document)
///     about the GroupPost (table) for more information.
/// </summary>
public enum GroupPostStatus
{
    Pending = 1,

    Approved = 2,

    Rejected = 3,
}
