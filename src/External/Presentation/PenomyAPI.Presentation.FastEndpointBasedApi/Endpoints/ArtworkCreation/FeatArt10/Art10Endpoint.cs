using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;
using System.Threading;
using System.Threading.Tasks;

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

    public async override Task<object> ExecuteAsync(
        CreateComicChapterRequestDto requestDto,
        CancellationToken ct)
    {
        return Result<object>.Success(null);
    }
}
