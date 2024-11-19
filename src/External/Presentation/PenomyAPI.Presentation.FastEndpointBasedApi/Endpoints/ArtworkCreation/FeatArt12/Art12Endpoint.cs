using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt12;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12;

public class Art12Endpoint : Endpoint<Art12RequestDto, Art12HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper = FormFileHelper.Instance;

    public override void Configure()
    {
        Post("art12/chapter/update");

        AllowAnonymous();

        AllowFormData();
        AllowFileUploads();
    }

    public override async Task<Art12HttpResponse> ExecuteAsync(Art12RequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequestDto(requestDto);
        var httpResponse = validationResult.Value;

        if (!validationResult.IsSuccess)
        {
            await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

            return httpResponse;
        }

        // Check if any image files are uploaded new, then map them to the MediaUpdatedInfoItems
        if (requestDto.HasNewUploadImageFiles()) requestDto.MapRelatedFormFilesIntoMediaUpdatedInfoItems();

        // Map the request from the DTO and execute.
        var creatorId = 123456789012345678;
        var mapToRequestResult = requestDto.TryMapToRequest(creatorId);

        if (!mapToRequestResult.IsSuccess)
        {
            httpResponse = Art12HttpResponseMapper.Map(Art12Response.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS);

            await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

            return httpResponse;
        }

        var featureRequest = mapToRequestResult.Value;
        var featureResponse = await FeatureExtensions.ExecuteAsync<Art12Request, Art12Response>(
            featureRequest,
            cancellationToken);

        httpResponse = Art12HttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }

    private static Result<Art12HttpResponse> ValidateRequestDto(Art12RequestDto requestDto)
    {
        Art12HttpResponse httpResponse;

        // Check if the json that uploaded in the ChapterMediaItemsInJson can be deserialized or not.
        var deserializedResult = requestDto.DeserializeChapterMediaUpdatedInfoItems();

        if (!deserializedResult.IsSuccess)
        {
            httpResponse = Art12HttpResponseMapper.Map(Art12Response.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS);

            return Result<Art12HttpResponse>.Failed(httpResponse);
        }

        // Validate all uploaded image files.
        var allUploadFilesValid = requestDto.ValidateAllUploadImageFiles();

        if (!allUploadFilesValid)
        {
            httpResponse = Art12HttpResponseMapper.Map(Art12Response.INVALID_FILE_FORMAT);

            return Result<Art12HttpResponse>.Failed(httpResponse);
        }

        return Result<Art12HttpResponse>.Success(default);
    }
}
