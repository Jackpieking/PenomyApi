using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt20;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt20;

public class Art20Endpoint : Endpoint<Art20CreateChapterRequestDto, Art20HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static Art20Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("art20/anime/chapter/create");
        AllowFormData();
        AllowFileUploads();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art20CreateChapterRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art20CreateChapterRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating a new chapter of the specified anime.";
            summary.Description = "This endpoint is used for creating a new chapter of the specified anime.";
            summary.ExampleRequest = new() { };
            summary.Response<Art10HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = Art20HttpResponse.GetAppCode(Art20ResponseAppCode.SUCCESS)
                }
            );
        });
    }

    public override async Task<Art20HttpResponse> ExecuteAsync(
        Art20CreateChapterRequestDto requestDto,
        CancellationToken ct
    )
    {
        Art20HttpResponse httpResponse;

        // Check if the upload thumbnail image not empty.
        var hasThumbnail = !Equals(requestDto.ThumbnailImageFile, null);

        Result<Art20HttpResponse> validationResult;
        if (hasThumbnail)
        {
            // Check if the upload thumbnail image file is valid or not.
            validationResult = InternalValidateImageFile(requestDto.ThumbnailImageFile);

            if (!validationResult.IsSuccess)
            {
                httpResponse = validationResult.Value;

                await SendAsync(httpResponse, httpResponse.HttpCode, ct);

                return httpResponse;
            }
        }

        // Check if the chapter image list is null or empty.
        if (Equals(requestDto.ChapterVideoFile, null))
        {
            httpResponse = Art20HttpResponse.INVALID_FILE_FORMAT;
        }

        // Check the upload video file.
        var isVideoFileValid = _formFileHelper.IsValidVideoFile(requestDto.ChapterVideoFile);

        if (!isVideoFileValid)
        {
            return Art20HttpResponse.INVALID_FILE_FORMAT;
        }

        if (requestDto.ChapterVideoFile.Length > ArtworkConstraints.MAXIMUM_VIDEO_FILE_SIZE)
        {
            return Art20HttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        //Excecute the request to create chapter.
        var request = requestDto.MapToRequest(creatorId);

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art20Request, Art20Response>(request, ct);

        httpResponse = Art20HttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    /// <summary>
    ///     Internally validate the input <paramref name="imageFile"/>,
    /// </summary>
    /// <param name="imageFile">
    ///     The image file to validate.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{Art10HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<Art20HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art20HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = Art20HttpResponse.INVALID_FILE_EXTENSION;

            return Result<Art20HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art20HttpResponse.INVALID_FILE_FORMAT;

            return Result<Art20HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art20HttpResponse.FILE_SIZE_IS_EXCEED_THE_LIMIT;

            return Result<Art20HttpResponse>.Failed(httpResponse);
        }

        return Result<Art20HttpResponse>.Success(default);
    }
}
