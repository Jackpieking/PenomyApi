using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatG36;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG36.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG36.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG36;

public sealed class G36Endpoint : Endpoint<G36UpdateProfileRequestDto, G36HttpResponse>
{
    private readonly IFormFileHelper _formFileHelper = FormFileHelper.Instance;

    public override void Configure()
    {
        Patch("g36/user/profile");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<G36UpdateProfileRequestDto>>();

        AllowFileUploads();
        AllowFormData();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for updating the current user profile.";
            summary.Description = "This endpoint is used for updating the current user profile.";
            summary.ExampleRequest = new() { NickName = "some new nick name", };
            summary.Response<Art3HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<G36HttpResponse> ExecuteAsync(
        G36UpdateProfileRequestDto requestDto,
        CancellationToken ct)
    {
        G36HttpResponse httpResponse;

        // Check if the upload image file is valid or not.
        if (requestDto.HasUploadAvatarFile())
        {
            var validationResult = InternalValidateImageFile(requestDto.AvatarFile);

            if (!validationResult.IsSuccess)
            {
                httpResponse = validationResult.Value;

                await SendAsync(httpResponse, httpResponse.HttpCode, ct);

                return httpResponse;
            }
        }

        var stateBag = ProcessorState<StateBag>();

        var request = new G36Request
        {
            UserId = stateBag.AppRequest.UserId,
            NickName = requestDto.NickName,
            AboutMe = requestDto.AboutMe,
        };

        if (requestDto.HasUploadAvatarFile())
        {
            request.AvatarFileInfo = new ImageFileInfo
            {
                FileDataStream = requestDto.AvatarFile.OpenReadStream(),
                FileName = requestDto.AvatarFile.FileName,
            };
        }

        var featResponse = await FeatureExtensions
            .ExecuteAsync<G36Request, G36Response>(request, ct);

        httpResponse = G36HttpResponse.MapFrom(featResponse);

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
    ///     The <see cref="Result{G36HttpResponse}"/> instance as the validation result.
    /// </returns>
    private Result<G36HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            return Result<G36HttpResponse>.Failed(G36HttpResponse.INVALID_FILE_UPLOAD);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            return Result<G36HttpResponse>.Failed(G36HttpResponse.INVALID_FILE_UPLOAD);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
        {
            return Result<G36HttpResponse>.Failed(G36HttpResponse.INVALID_FILE_UPLOAD);
        }

        return Result<G36HttpResponse>.Success(default);
    }
}
