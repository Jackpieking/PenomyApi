using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

/// <summary>
///     Store the original country of the artworks.
/// </summary>
public sealed class ArtworkOrigin :
    EntityWithId<long>,
    ICreatedEntity<long>,
    IUpdatedEntity<long>
{
    /// <summary>
    ///     The name of the country.
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    ///     The label of the artwork corresponding to its original country.
    ///     The label is just support for comic type. For example:
    ///     <br/>
    ///     1. Origin(Japanese): Manga <br/>
    ///     2. Origin(Korean): Manhwa <br/>
    ///     3. Origin(Chinese): Manhua <br/>
    /// </summary>
    /// <remarks>
    ///     For more information: https://allanimangas.medium.com/manga-manhwa-and-manhua-whats-the-difference-651e29a96d
    /// </remarks>
    public string Label { get; set; }

    public string ImageUrl { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SystemAccount Creator { get; set; }

    public SystemAccount Updater { get; set; }

    public IEnumerable<Artwork> Artworks { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int CountryNameLength = 64;

        public const int LabelLength = 64;

        public const int ImageUrlLength = 2000;
    }
    #endregion
}
