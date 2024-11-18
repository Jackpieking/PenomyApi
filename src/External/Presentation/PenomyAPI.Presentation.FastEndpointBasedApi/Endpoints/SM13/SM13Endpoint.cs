using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.App.SM13;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13;

public class Sm13Endpoint : Endpoint<SM13RequestDto, SM13HttpResponse>
{
    private static readonly IFormFileHelper FormFileHelper;

    static Sm13Endpoint()
    {
        FormFileHelper = Helpers.IFormFiles.FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Patch("/SM13/post/edit");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM13RequestDto>>();
        AllowFileUploads();
        AllowFormData();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for edit an user post";
            summary.Description = "This endpoint is used for editing an user post.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Art4HttpResponse { AppCode = Art4ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM13HttpResponse> ExecuteAsync(
        SM13RequestDto requestDto,
        CancellationToken ct
    )
    {
        List<AppFileInfo> mediaFiles = [];
        var stateBag = ProcessorState<StateBag>();
        SM13HttpResponse response = new();
        if (requestDto.IsAttachedMediaUpdated())
            foreach (var media in requestDto.AttachedMedia)
            {
                var fileExtension = Helpers.IFormFiles.FormFileHelper.Instance.GetFileExtension(media);
                if (!FormFileHelper.IsValidImageFile(media))
                    return new SM13HttpResponse
                    {
                        AppCode = SM13HttpResponse.GetAppCode(SM13ResponseStatusCode.INVALID_FILE_EXTENSION),
                        HttpCode = StatusCodes.Status400BadRequest,
                        Errors = "Invalid file format"
                    };
                var result = InternalValidateImageFile(media);
                var fileInfo = new ImageFileInfo
                {
                    FileId = $"{requestDto.AttachedMedia}{media.Length}",
                    FileDataStream = media.OpenReadStream(),
                    FileName = media.FileName,
                    FileExtension = fileExtension
                };
                if (result.IsSuccess) mediaFiles.Add(fileInfo);
            }

        var userId = stateBag.AppRequest.UserId;
        SM13Request request = new()
        {
            UserId = userId,
            UserPostId = requestDto.PostId,
            AllowComment = requestDto.AllowComment,
            PublicLevel = requestDto.PublicLevel,
            Content = requestDto.Content,
            AppFileInfos = mediaFiles,
            IsUpdateAttachMedia = requestDto.IsAttachedMediaUpdated()
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<SM13Request, SM13Response>(
            request,
            ct
        );

        var httpResponse = SM13HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    private Result<SM13HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        SM13HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!FormFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = SM13HttpResponseManager
                .Resolve(SM13ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<SM13HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!FormFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = SM13HttpResponseManager
                .Resolve(SM13ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<SM13HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length <= ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
            return Result<SM13HttpResponse>.Success(default);
        httpResponse = SM13HttpResponseManager
            .Resolve(SM13ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
            .Invoke(default);

        return Result<SM13HttpResponse>.Failed(httpResponse);
    }
}
