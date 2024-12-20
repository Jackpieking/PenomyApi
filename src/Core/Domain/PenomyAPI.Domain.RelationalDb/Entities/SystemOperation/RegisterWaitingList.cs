using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SystemOperation;

/// <summary>
///     This table supports to create a waiting list
///     before user succesfully register an account in the
///     system with their mail.
/// </summary>
public sealed class RegisterWaitingList : EntityWithId<long>
{
    public string Email { get; set; }

    public SendMailStatus SendMailStatus { get; set; }

    public DateTime LastMailSentAt { get; set; }

    public DateTime NextMailSentAt { get; set; }

    #region MetaData
    public static class MetaData
    {
        public const int EmailLength = 256;
    }
    #endregion
}
