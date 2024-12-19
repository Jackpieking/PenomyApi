using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt10;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponseManagers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt10;

public class Art10Endpoint : Endpoint<Art10CreateChapterRequestDto, Art10HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static Art10Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Post("art10/chapter/create");
        AllowFormData();
        AllowFileUploads();

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art10CreateChapterRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art10CreateChapterRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating a new chapter of the specified comic.";
            summary.Description = "This endpoint is used for creating a new chapter of the specified comic.";
            summary.ExampleRequest = new() { };
            summary.Response<Art10HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"ART10.{Art10ResponseAppCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<Art10HttpResponse> ExecuteAsync(
        Art10CreateChapterRequestDto requestDto,
        CancellationToken ct
    )
    {
        Art10HttpResponse httpResponse;

        // Check if the upload thumbnail image not empty.
        var hasThumbnail = !Equals(requestDto.ThumbnailImageFile, null);

        Result<Art10HttpResponse> validationResult;
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
        var hasChapterImageList = Equals(requestDto.ChapterImageFiles, null);

        if (!hasChapterImageList)
        {
            httpResponse = Art10HttpResponseManager
                .Resolve(Art10ResponseAppCode.CHAPTER_IMAGE_LIST_IS_EMPTY)
                .Invoke(default);
        }

        // Validate all chapter image files.
        validationResult = InternalValidateMultipleImageFile(requestDto.ChapterImageFiles);

        if (!validationResult.IsSuccess)
        {
            httpResponse = validationResult.Value;

            await SendAsync(httpResponse, httpResponse.HttpCode, ct);

            return httpResponse;
        }

        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        // Excecute the request to create chapter.
        var request = requestDto.MapToRequest(creatorId);

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art10Request, Art10Response>(request, ct);

        httpResponse = Art10HttpResponseManager
            .Resolve(featureResponse.AppCode)
            .Invoke(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    /// <summary>
    ///     Internally validate the list of <paramref name="imageFiles"/>.
    /// </summary>
    /// <param name="imageFiles"></param>
    private Result<Art10HttpResponse> InternalValidateMultipleImageFile(
        IFormFileCollection imageFiles)
    {
        foreach (var imageFile in imageFiles)
        {
            var validtionResult = InternalValidateImageFile(imageFile);

            if (!validtionResult.IsSuccess)
            {
                var httpResponse = Art10HttpResponseManager
                .Resolve(Art10ResponseAppCode.INVALID_FILE_FORMAT)
                .Invoke(default);

                return Result<Art10HttpResponse>.Failed(httpResponse);
            }
        }

        return Result<Art10HttpResponse>.Success(default);
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
    private Result<Art10HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        Art10HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = Art10HttpResponseManager
                .Resolve(Art10ResponseAppCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<Art10HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = Art10HttpResponseManager
                .Resolve(Art10ResponseAppCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<Art10HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            httpResponse = Art10HttpResponseManager
                .Resolve(Art10ResponseAppCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
                .Invoke(default);

            return Result<Art10HttpResponse>.Failed(httpResponse);
        }

        return Result<Art10HttpResponse>.Success(default);
    }
}
