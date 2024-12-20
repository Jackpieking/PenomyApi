using System;
using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountResponse : IFeatureResponse
{
    /// <summary>
    ///     Page size will limit the total items to
    ///     display per page when pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 8;

    public int TotalArtworks { get; set; }

    public G28PageCountResponseStatusCode StatusCode { get; set; }

    public int TotalPages { get; set; }

    public int PageSize => DEFAULT_PAGE_SIZE;

    public bool IsPagination { get; set; }

    /// <summary>
    ///     Calculate the total pages to display when pagination
    ///     and return the pagination options response contains
    ///     the detail based on the input <paramref name="totalArtworks"/>.
    /// </summary>
    /// <param name="totalArtworks">
    ///     The total chapters of the comic to calculate the pagination options.
    /// </param>
    public static G28PageCountResponse CalculateAndReturn(int totalArtworks)
    {
        int totalPages = (int) Math.Ceiling((double) totalArtworks / DEFAULT_PAGE_SIZE);
        var isPagination = totalPages > 1;

        return new()
        {
            StatusCode = G28PageCountResponseStatusCode.SUCCESS,
            TotalArtworks = totalArtworks,
            IsPagination = isPagination,
            TotalPages = totalPages,
        };
    }
}
