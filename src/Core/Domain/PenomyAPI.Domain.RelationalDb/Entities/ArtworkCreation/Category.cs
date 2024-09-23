using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class Category : EntityWithId<long>, ICreatedEntity<long>, IUpdatedEntity<long>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 32;

        public const int DescriptionLength = 100;
    }
    #endregion
}
