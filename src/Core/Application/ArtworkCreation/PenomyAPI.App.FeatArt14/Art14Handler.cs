using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt14;

public sealed class Art14Handler
    : IFeatureHandler<Art14Request, Art14Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt14Repository _art14Repository;

    public Art14Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art14Response> ExecuteAsync(Art14Request request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;

        _art14Repository = unitOfWork.Art14Repository;

        var isChapterExisted = await _art14Repository.IsChapterExistedAsync(request.ChapterId, ct);

        if (!isChapterExisted)
        {
            return Art14Response.CHAPTER_ID_NOT_FOUND;
        }

        var creatorHasPermission = await _art14Repository.IsCurrentCreatorHasPermissionAsync(
            request.CreatorId,
            request.ChapterId,
            ct);

        if (!creatorHasPermission)
        {
            return Art14Response.CREATOR_HAS_NO_PERMISSION;
        }

        var removeResult = await _art14Repository.RemoveArtworkChapterByIdAsync(
            creatorId: request.CreatorId,
            artworkId: request.ArtworkId,
            chapterId: request.ChapterId,
            cancellationToken: ct);

        if (removeResult)
        {
            return Art14Response.SUCCESS;
        }

        return Art14Response.DATABASE_ERROR;
    }
}
