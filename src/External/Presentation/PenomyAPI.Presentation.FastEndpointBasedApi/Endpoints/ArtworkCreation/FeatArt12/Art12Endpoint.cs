using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt12;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12;

public class Art12Endpoint : Endpoint<Art12RequestDto, Art12HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper = FormFileHelper.Instance;

    public override void Configure()
    {
        Post("art12/chapter/update");

        AllowFormData();
        AllowFileUploads();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art12RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art12RequestDto>>();
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

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        // Map the request from the DTO and execute.
        var mapToRequestResult = requestDto.TryMapToRequest(creatorId);

        if (!mapToRequestResult.IsSuccess)
        {
            httpResponse = Art12HttpResponseMapper.Map(
                Art12Response.INVALID_JSON_SCHEMA_FROM_INPUT_MEDIA_ITEMS);

            await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

            return httpResponse;
        }

        // Execute the create chapter handler.
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
