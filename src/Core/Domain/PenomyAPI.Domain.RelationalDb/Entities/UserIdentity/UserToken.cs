using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

public sealed class UserToken
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public string LoginProvider { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int LoginProviderLength = 200;

        public const int NameLength = 200;

        public const int ValueLength = 100;
    }
    #endregion
}
