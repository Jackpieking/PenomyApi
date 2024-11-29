using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.HttpResponse;

public class G45HttpResponse
    : AppHttpResponse<IEnumerable<G45FollowedArtworkResponseDto>> { }
