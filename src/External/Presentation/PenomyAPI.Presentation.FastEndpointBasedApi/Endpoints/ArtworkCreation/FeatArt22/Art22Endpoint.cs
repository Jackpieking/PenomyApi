using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt22;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22;

public class Art22Endpoint
    : Endpoint<Art22UpdateAnimeChapterRequestDto, Art22UpdateAnimeChapterHttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper = FormFileHelper.Instance;

    public override void Configure()
    {
        Post("art22/chapter/update");

        AllowFormData();
        AllowFileUploads();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art22UpdateAnimeChapterRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art22UpdateAnimeChapterRequestDto>>();
    }

    public override async Task<Art22UpdateAnimeChapterHttpResponse> ExecuteAsync(
        Art22UpdateAnimeChapterRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var validationResult = ValidateRequestDto(requestDto);
        var httpResponse = validationResult.Value;

        if (!validationResult.IsSuccess)
        {
            await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

            return httpResponse;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        // Map the request from the DTO and execute.
        var featureRequest = requestDto.MapToRequest(creatorId);

        // Execute the create chapter handler.
        var featureResponse = await FeatureExtensions.ExecuteAsync<Art22Request, Art22Response>(
            featureRequest,
            cancellationToken);

        httpResponse = Art22UpdateAnimeChapterHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }

    private static Result<Art22UpdateAnimeChapterHttpResponse> ValidateRequestDto(
        Art22UpdateAnimeChapterRequestDto requestDto)
    {
        if (requestDto.HasThumbnailImageFile())
        {
            var isValidImageFile = _formFileHelper.IsValidImageFile(requestDto.ThumbnailImageFile);

            if (!isValidImageFile)
            {
                return Result<Art22UpdateAnimeChapterHttpResponse>
                    .Failed(Art22UpdateAnimeChapterHttpResponse.INVALID_FILE_FORMAT);
            }

            var isFileExceedLimitSize = requestDto.ThumbnailImageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE;

            if (isFileExceedLimitSize)
            {
                return Result<Art22UpdateAnimeChapterHttpResponse>
                    .Failed(Art22UpdateAnimeChapterHttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT);
            }
        }

        if (requestDto.HasChapterVideoFile())
        {
            var isValidImageFile = _formFileHelper.IsValidVideoFile(requestDto.ChapterVideoFile);

            if (!isValidImageFile)
            {
                return Result<Art22UpdateAnimeChapterHttpResponse>
                    .Failed(Art22UpdateAnimeChapterHttpResponse.INVALID_FILE_FORMAT);
            }

            var isFileExceedLimitSize = requestDto.ChapterVideoFile.Length > ArtworkConstraints.MAXIMUM_VIDEO_FILE_SIZE;

            if (isFileExceedLimitSize)
            {
                return Result<Art22UpdateAnimeChapterHttpResponse>
                    .Failed(Art22UpdateAnimeChapterHttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT);
            }
        }

        return Result<Art22UpdateAnimeChapterHttpResponse>.Success(default);
    }
}
