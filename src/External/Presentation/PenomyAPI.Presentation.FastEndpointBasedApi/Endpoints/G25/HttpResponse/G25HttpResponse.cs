using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse
{
    public class G25HttpResponse : AppHttpResponse<List<G25ResponseDto>>
    {
        public List<G25ResponseDto> g25ResponseDtos { get; set; }
    }
}
