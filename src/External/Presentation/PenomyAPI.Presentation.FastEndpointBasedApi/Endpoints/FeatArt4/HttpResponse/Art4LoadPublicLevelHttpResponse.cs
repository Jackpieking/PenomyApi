using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse;

public sealed class Art4LoadPublicLevelHttpResponse
    : AppHttpResponse<IEnumerable<PublicLevelDto>>
{
}
