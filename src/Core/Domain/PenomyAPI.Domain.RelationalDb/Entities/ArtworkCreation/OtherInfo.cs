using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

/// <summary>
///     This class is support to provide more
///     information about an <see cref="Artwork"/>
/// </summary>
public sealed class OtherInfo : EntityWithId<long>
{
    public string Name { get; set; }

    public string DataType { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public IEnumerable<ArtworkOtherInfo> ArtworkOtherInfos { get; set; }

    public SystemAccount Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 200;

        public const int DataTypeLength = 100;

        public const int DescriptionLength = 640;
    }
    #endregion
}
