using PenomyAPI.App.Common;
using System;

namespace PenomyAPI.App.FeatG8.OtherHandlers;

public sealed class G8GetPaginationOptionsResponse : IFeatureResponse
{
    /// <summary>
    ///     Page size will limit the total items to
    ///     display per page when pagination.
    /// </summary>
    public const int DEFAULT_PAGE_SIZE = 10;

    public G8ResponseStatusCode StatusCode { get; set; }

    public int TotalChapters { get; set; }

    public int TotalPages { get; set; }

    public int PageSize => DEFAULT_PAGE_SIZE;

    public bool IsPagination { get; set; }

    public static readonly G8GetPaginationOptionsResponse COMIC_IS_NOT_FOUND = new()
    {
        StatusCode = G8ResponseStatusCode.NOT_FOUND,
    };

    /// <summary>
    ///     Calculate the total pages to display when pagination
    ///     and return the pagination options response contains
    ///     the detail based on the input <paramref name="totalChapters"/>.
    /// </summary>
    /// <param name="totalChapters">
    ///     The total chapters of the comic to calculate the pagination options.
    /// </param>
    public static G8GetPaginationOptionsResponse CalculateAndReturn(int totalChapters)
    {
        int totalPages = (int) Math.Ceiling((double) totalChapters / DEFAULT_PAGE_SIZE);
        var isPagination = totalPages > 1;

        return new()
        {
            StatusCode = G8ResponseStatusCode.SUCCESS,
            TotalChapters = totalChapters,
            IsPagination = isPagination,
            TotalPages = totalPages,
        };
    }
}
