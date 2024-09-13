using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class SocialGroupLinkedChatGroup : IEntity
{
    public long SocialGroupId { get; set; }

    public long ChatGroupId { get; set; }

    #region Navigation
    public SocialGroup LinkedSocialGroup { get; set; }

    public ChatGroup LinkedChatGroup { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
