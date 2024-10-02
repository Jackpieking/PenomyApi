using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemOperation;

public sealed class MailServerStatus : EntityWithId<long>
{
    public string MailDomain { get; set; }

    public int LeftMailQuota { get; set; }

    public ServerStatus ServerStatus { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int MailDomainLength = 256;
    }
    #endregion
}
