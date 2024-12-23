using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Infra.Configuration.Options;

namespace PenomyAPI.App.SM14;

public class SM14Handler : IFeatureHandler<SM14Request, SM14Response>
{
    private readonly Lazy<IDefaultDistributedFileService> _fileService;
    private readonly CloudinaryOptions _options;
    private readonly ISM14Repository _sm14Repository;

    public SM14Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IDefaultDistributedFileService> fileService,
        CloudinaryOptions options
    )
    {
        _sm14Repository = unitOfWork.Value.FeatSM14Repository;
        _fileService = fileService;
        _options = options;
    }

    public async Task<SM14Response> ExecuteAsync(SM14Request request, CancellationToken ct)
    {
        bool result;
        if (
            !request.IsGroupPost
            && !await _sm14Repository.IsExistUserPostAsync(request.PostId, request.UserId, ct)
        )
            return SM14Response.NOT_FOUND;

        List<long> postMediaIds;

        // Get media ids
        if (request.IsGroupPost)
            postMediaIds = await _sm14Repository.GetGroupPostAttachedFileIdAsync(
                request.PostId,
                ct
            );
        else
            postMediaIds = await _sm14Repository.GetAttachedFileIdAsync(request.PostId, ct);

        if (postMediaIds.Count != 0)
        {
            bool isRemoveSuccess;
            foreach (var fileId in postMediaIds)
            {
                isRemoveSuccess = await _fileService.Value.DeleteFileByIdAsync(
                    $"posts/{request.PostId / fileId}",
                    ct
                );
                if (!isRemoveSuccess)
                    return SM14Response.FILE_SERVICE_ERROR;
            }
        }

        result = await _sm14Repository.RemoveUserPostAsync(
            request.PostId,
            request.UserId,
            request.IsGroupPost,
            ct
        );
        return result ? SM14Response.SUCCESS : SM14Response.DATABASE_ERROR;
    }
}
