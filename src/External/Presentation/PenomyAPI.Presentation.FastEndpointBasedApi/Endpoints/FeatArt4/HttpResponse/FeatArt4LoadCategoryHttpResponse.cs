using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.HttpResponse
{
    public class FeatG3LoadCategoryHttpResponse
    {
        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}
