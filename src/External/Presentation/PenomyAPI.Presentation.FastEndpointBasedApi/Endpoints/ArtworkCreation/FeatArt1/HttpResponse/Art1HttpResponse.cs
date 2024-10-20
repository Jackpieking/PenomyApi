using PenomyAPI.App.FeatArt1;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;

public sealed class Art1HttpResponse : AppHttpResponse<IEnumerable<ArtworkDetailResponseDto>>
{
    public bool AllowPagination { get; set; }

    public int TotalPages { get; set; }

    public static string GetAppCode(Art1ResponseAppCode appCode)
    {
        return $"Art1.{appCode}.{(int) appCode}";
    }
}
