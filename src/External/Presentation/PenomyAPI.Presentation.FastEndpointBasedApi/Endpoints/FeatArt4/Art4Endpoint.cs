using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.Helpers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4;

public class Art4Endpoint : Endpoint<Art4RequestDto, IResult>
{
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public Art4Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

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
            summary.Response<Art4HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = Art4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<IResult> ExecuteAsync(
        Art4RequestDto requestDto,
        CancellationToken ct
    )
    {
        // Check if input categories has valid json schema or not.
        if (!requestDto.IsValidSelectedCategoriesInput())
        {
            var responseBody = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_JSON_SCHEMA_FROM_INPUT_CATEGORIES)
                .Invoke();

            return Results.BadRequest(responseBody);
        }

        // Check if the upload thumbnail image is valid or not.
        if (!requestDto.IsValidThumbnailImageFileInput())
        {
            var responseBody = Art4HttpResponseManager
                .Resolve(Art4ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke();

            return Results.BadRequest(responseBody);
        }

        // Generate a random id for comic.
        var comicId = _idGenerator.Value.Get();
        const long testUserId = 123456789012345678;
        var fileExtension = IFormFileHelper.GetFileExtension(requestDto.ThumbnailImageFile);

        // Create request and send to handler
        const string thumbnailImageName = "thumbnail";

        var featArt4Request = new Art4Request
        {
            ComicId = comicId,
            Title = requestDto.Title,
            Introduction = requestDto.Introduction,
            OriginId = requestDto.OriginId,
            PublicLevel = requestDto.PublicLevel,
            AllowComment = requestDto.AllowComment,
            ArtworkCategories = requestDto.ArtworkCategories.Select(category => new ArtworkCategory
            {
                ArtworkId = comicId,
                CategoryId = long.Parse(category.Id),
            }),
            AuthorName = "Default",
            CreatedBy = testUserId,
            ThumbnailFileInfo = new ImageFileInfo
            {
                FileId = comicId.ToString(),
                FileName = $"{thumbnailImageName}.{fileExtension}",
                FileDataStream = requestDto.ThumbnailImageFile.OpenReadStream(),
            }
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Art4Request, Art4Response>(
            featArt4Request,
            ct
        );

        var httpResponse = Art4HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke();

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new Art4ResponseDto { ComicId = featArt4Request.ComicId, };

            return Results.Ok(httpResponse);
        }

        return Results.BadRequest(httpResponse);
    }
}
