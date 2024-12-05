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
using PenomyAPI.App.SM12;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12;

public class SM12Endpoint : Endpoint<SM12RequestDto, SM12HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    static SM12Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public SM12Endpoint(Lazy<ISnowflakeIdGenerator> idGenerator)
    {
        _idGenerator = idGenerator;

        var id = _idGenerator.Value.Get();
    }

    public override void Configure()
    {
        Post("/SM12/post/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM12RequestDto>>();
        AllowFormData();
        AllowFileUploads();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creating a new user post";
            summary.Description = "This endpoint is used for creating new user post.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM12HttpResponse { AppCode = SM12ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM12HttpResponse> ExecuteAsync(
        SM12RequestDto requestDto,
        CancellationToken ct
    )
    {
        List<AppFileInfo> mediaFiles = [];
        var stateBag = ProcessorState<StateBag>();
        SM12HttpResponse response = new();
        if (requestDto.AttachedMedia != null)
            foreach (var media in requestDto.AttachedMedia)
            {
                var fileExtension = FormFileHelper.Instance.GetFileExtension(media);
                if (!_formFileHelper.IsValidImageFile(media))
                    return new SM12HttpResponse
                    {
                        AppCode = SM12HttpResponse.GetAppCode(SM12ResponseStatusCode.INVALID_FILE_EXTENSION),
                        HttpCode = StatusCodes.Status400BadRequest,
                        Errors = "Invalid file format"
                    };
                var result = InternalValidateImageFile(media);
                var fileInfo = new ImageFileInfo
                {
                    FileId = _idGenerator.Value.Get().ToString(),
                    FileDataStream = media.OpenReadStream(),
                    FileName = media.FileName,
                    FileExtension = fileExtension
                };
                if (result.IsSuccess) mediaFiles.Add(fileInfo);
            }

        var userPostId = _idGenerator.Value.Get();
        var userId = stateBag.AppRequest.UserId;
        SM12Request request = new()
        {
            UserId = userId,
            UserPostId = userPostId,
            AllowComment = requestDto.AllowComment,
            PublicLevel = requestDto.PublicLevel,
            Content = requestDto.Title,
            AppFileInfos = mediaFiles
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<SM12Request, SM12Response>(
            request,
            ct
        );

        var httpResponse = SM12HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }

    private Result<SM12HttpResponse> InternalValidateImageFile(IFormFile imageFile)
    {
        SM12HttpResponse httpResponse;

        // Check if the file extension is valid or not.
        if (!_formFileHelper.HasValidExtension(imageFile, ArtworkConstraints.VALID_FILE_EXTENSIONS))
        {
            httpResponse = SM12HttpResponseManager
                .Resolve(SM12ResponseStatusCode.INVALID_FILE_EXTENSION)
                .Invoke(default);

            return Result<SM12HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is really an image file or not.
        if (!_formFileHelper.IsValidImageFile(imageFile))
        {
            httpResponse = SM12HttpResponseManager
                .Resolve(SM12ResponseStatusCode.INVALID_FILE_FORMAT)
                .Invoke(default);

            return Result<SM12HttpResponse>.Failed(httpResponse);
        }

        // Check if the uploaded file is exceed the size limit or not.
        if (imageFile.Length <= ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE)
            return Result<SM12HttpResponse>.Success(default);
        httpResponse = SM12HttpResponseManager
            .Resolve(SM12ResponseStatusCode.FILE_SIZE_IS_EXCEED_THE_LIMIT)
            .Invoke(default);

        return Result<SM12HttpResponse>.Failed(httpResponse);
    }
}
