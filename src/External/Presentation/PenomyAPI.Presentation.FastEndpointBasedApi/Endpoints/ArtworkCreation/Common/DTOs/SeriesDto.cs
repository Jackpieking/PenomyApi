using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;

public sealed class SeriesDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public static SeriesDto MapFrom(IEnumerable<ArtworkSeries> artworkSeries)
    {
        if (Equals(artworkSeries, default))
        {
            return default;
        }

        var series = artworkSeries.FirstOrDefault();

        return new()
        {
            Id = series.SeriesId.ToString(),
            Title = series.Series.Title,
        };
    }
}
