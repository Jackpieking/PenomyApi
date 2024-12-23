using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG8;

public class G8Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<ArtworkChapter> Chapters { get; set; }
    public int ChapterCount { get; set; }
    public G8ResponseStatusCode StatusCode { get; set; }

    public static readonly G8Response ARTWORK_IS_NOT_FOUND =
        new() { StatusCode = G8ResponseStatusCode.NOT_FOUND, };
}
