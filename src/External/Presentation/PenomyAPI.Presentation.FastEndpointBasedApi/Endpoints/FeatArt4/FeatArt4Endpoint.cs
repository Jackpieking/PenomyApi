using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4;

public class FeatArt4Endpoint : Endpoint<FeatArt4RequestDto, FeatArt4HttpResponse>
{
    public override void Configure()
    {
        Post("/art4/comic/create");

        AllowAnonymous();
        AllowFormData();
        AllowFileUploads();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating a new comic";
            summary.Description = "This endpoint is used for creating new comic purpose.";
            summary.Response<FeatArt4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = FeatArt4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<FeatArt4HttpResponse> ExecuteAsync(FeatArt4RequestDto requestDto, CancellationToken ct)
    {
        var featArt4Request = new FeatArt4Request
        {
            ComicId = 1,
            Title = requestDto.Title,
            Introduction = requestDto.Introduction,
            OriginId = requestDto.OriginId,
            PublicLevel = requestDto.PublicLevel,
            AllowComment = requestDto.AllowComment,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<FeatArt4Request, FeatArt4Response>(featArt4Request, ct);

        var httpResponse = FeatArt4HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featArt4Request, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new FeatArt4ResponseDto
            {
                ComicId = featArt4Request.ComicId,
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
