using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10;

public class Art10Endpoint : Endpoint<CreateComicChapterRequestDto>
{
    public override void Configure()
    {
        Post("art10/chapter/create");

        AllowAnonymous();
        AllowFormData();
        AllowFileUploads();
    }

    public override async Task<object> ExecuteAsync(
        CreateComicChapterRequestDto requestDto,
        CancellationToken ct
    )
    {
        return Result<object>.Success(null);
    }
}
