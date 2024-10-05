using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkViolationFlagType
    : EntityWithId<long>,
        ICreatedEntity<long>,
        IUpdatedEntity<long>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }

    public IEnumerable<ArtworkViolationFlag> ArtworkViolationFlags { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 200;

        public const int DescriptionLength = 200;
    }
    #endregion
}
