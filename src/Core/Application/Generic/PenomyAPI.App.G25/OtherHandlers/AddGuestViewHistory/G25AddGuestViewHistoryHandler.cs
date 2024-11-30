using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.AddGuestViewHistory;

public sealed class G25AddGuestViewHistoryHandler
    : IFeatureHandler<G25AddGuestViewHistoryRequest, G25AddGuestViewHistoryResponse>
{
    private IUnitOfWork _unitOfWork;
    private readonly IG25Repository _g25Repository;
    private IArtworkChapterRepository _chapterRepository;

    public G25AddGuestViewHistoryHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
        _g25Repository = _unitOfWork.G25Repository;
    }

    public async Task<G25AddGuestViewHistoryResponse> ExecuteAsync(
        G25AddGuestViewHistoryRequest request,
        CancellationToken ct)
    {
        var isGuestIdExisted = await _g25Repository.IsGuestIdExistedAsync(request.GuestId, ct);

        if (!isGuestIdExisted)
        {
            return G25AddGuestViewHistoryResponse.GUEST_ID_NOT_FOUND;
        }

        // Check if provided chapter detail is found or not.
        _chapterRepository = _unitOfWork.ChapterRepository;

        const int GUEST_ID = -1;

        var isArtworkChapterExisted = await _chapterRepository.IsChapterAvailableToDisplayByIdAsync(
            request.ArtworkId,
            request.ChapterId,
            GUEST_ID);

        if (!isArtworkChapterExisted)
        {
            return G25AddGuestViewHistoryResponse.CHAPTER_IS_NOT_FOUND;
        }

        var isViewHistoryRecordExisted = await _g25Repository.IsGuestViewHistoryRecordExistedAsync(
            request.GuestId,
            request.ArtworkId,
            ct);

        if (isViewHistoryRecordExisted)
        {
            return await UpdateViewHistoryRecordAsync(request, ct);
        }

        return await AddViewHistoryRecordAsync(request, ct);
    }

    private async Task<G25AddGuestViewHistoryResponse> UpdateViewHistoryRecordAsync(
        G25AddGuestViewHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var viewedAt = DateTime.UtcNow;

        var updateResult = await _g25Repository.UpdateGuestViewHistoryAsync(
            request.GuestId,
            request.ArtworkId,
            request.ChapterId,
            viewedAt,
            cancellationToken);

        if (updateResult)
        {
            return G25AddGuestViewHistoryResponse.SUCCESS;
        }

        return G25AddGuestViewHistoryResponse.DATABASE_ERROR;
    }

    private async Task<G25AddGuestViewHistoryResponse> AddViewHistoryRecordAsync(
        G25AddGuestViewHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var artworkType = await _g25Repository.GetArtworkTypeForAddingViewHistoryRecordAsync(
            request.ArtworkId,
            cancellationToken);

        var viewedAt = DateTime.UtcNow;

        var addResult = await _g25Repository.AddGuestViewHistoryAsync(
            request.GuestId,
            artworkType,
            request.ArtworkId,
            request.ChapterId,
            viewedAt,
            cancellationToken);

        if (addResult)
        {
            return G25AddGuestViewHistoryResponse.SUCCESS;
        }

        return G25AddGuestViewHistoryResponse.DATABASE_ERROR;
    }
}
