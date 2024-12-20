using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.SM34;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34;

public class SM34Endpoint : Endpoint<SM34RequestDto, SM34HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    static SM34Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public SM34Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public override void Configure()
    {
        Post("/SM34/group-post/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM34RequestDto>>();
        AllowFormData();
        AllowFileUploads();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating a new group post";
            summary.Description = "This endpoint is used for creating new group post.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM34HttpResponse
                {
                    AppCode = SM34ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<SM34HttpResponse> ExecuteAsync(
        SM34RequestDto requestDto,
        CancellationToken ct
    )
    {
        List<AppFileInfo> mediaFiles = [];
        var stateBag = ProcessorState<StateBag>();
        if (requestDto.AttachedMedia != null)
            foreach (var media in requestDto.AttachedMedia)
            {
                var fileExtension = FormFileHelper.Instance.GetFileExtension(media);
                if (!_formFileHelper.IsValidImageFile(media))
                    return new SM34HttpResponse
                    {
                        AppCode = SM34HttpResponse.GetAppCode(
                            SM34ResponseStatusCode.INVALID_FILE_EXTENSION
                        ),
                        HttpCode = StatusCodes.Status400BadRequest,
                        Errors = "Invalid file format",
                    };
                var result = InternalValidateImageFile(media);
                var fileInfo = new ImageFileInfo
                {
                    FileId = _idGenerator.Value.Get().ToString(),
                    FileDataStream = media.OpenReadStream(),
                    FileName = media.FileName,
                    FileExtension = fileExtension,
                };
                if (result.IsSuccess)
                    mediaFiles.Add(fileInfo);
            }

        var groupPostId = _idGenerator.Value.Get();
        var userId = stateBag.AppRequest.UserId;
        SM34Request request =
            new()
            {
                UserId = userId,
                GroupPostId = groupPostId,
                AllowComment = requestDto.AllowComment,
                GroupId = long.Parse(requestDto.GroupId),
                Content = requestDto.Title,
                AppFileInfos = mediaFiles,
            };
        var featResponse = await FeatureExtensions.ExecuteAsync<SM34Request, SM34Response>(
            request,
            ct
        );

        var httpResponse = SM34HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    private Result<SM34HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        SM34HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_IMAGE_FILE_EXTENSIONS))
        {
            httpResponse = SM34HttpResponseManager
                .Resolve(SM34ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<SM34HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = SM34HttpResponseManager
                .Resolve(SM34ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<SM34HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length <= ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
            return Result<SM34HttpResponse>.Success(default);
        httpResponse = SM34HttpResponseManager
            .Resolve(SM34ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
            .Invoke(default);

        return Result<SM34HttpResponse>.Failed(httpResponse);
    }
}
