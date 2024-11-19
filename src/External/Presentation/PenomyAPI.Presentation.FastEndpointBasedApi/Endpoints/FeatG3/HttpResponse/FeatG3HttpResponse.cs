using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;

public class FeatG3HttpResponse
    : AppHttpResponse<IEnumerable<G3ResponseItemDto>>
{
}