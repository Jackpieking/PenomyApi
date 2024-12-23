﻿using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;

public class RecommendedArtworkByCategory
{
    /// <summary>
    ///     The recommended category for the user.
    /// </summary>
    public RecommendedCategoryReadModel Category { get; set; }

    /// <summary>
    ///     The list of comics that belonged to the recommended category.
    /// </summary>
    public IEnumerable<RecommendedArtworkReadModel> RecommendedArtworks { get; set; }
}
