﻿using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG15;

public class G15AnimeDetailReadModel
{
    public long Id { get; set; }

    public string Title { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public long CountryId { get; set; }

    public string CountryName { get; set; }

    public bool HasSeries { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    /// <summary>
    ///     Allow the users to comment at the
    ///     detail page of this artwork.
    /// </summary>
    public bool AllowComment { get; set; }

    public IEnumerable<G5CategoryReadModel> ArtworkCategories { get; set; }

    public ArtworkMetaData ArtworkMetaData { get; set; }

    // Creator detail section.
    public long CreatorId { get; set; }

    public ArtworkSeries ArtworkSeries { get; set; }

    public long TotalChapters { get; set; }
}
