using PenomyAPI.App.FeatG2;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG2.HttpResponses;

public sealed class G2HttpResponse
    : AppHttpResponse<IEnumerable<G2RecommendedArtworkItemResponseDto>>
{
    public static string GetAppCode(G2ResponseAppCode appCode)
    {
        return $"G2.{appCode}.{(int) appCode}";
    }
}
