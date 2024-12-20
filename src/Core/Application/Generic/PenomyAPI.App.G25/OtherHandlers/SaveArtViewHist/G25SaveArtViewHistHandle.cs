using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;

public class G25SaveArtViewHistHandle
    : IFeatureHandler<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse>
{
    private readonly IG25Repository _g25Repository;
    private readonly IUnitOfWork _unitOfWork;

    public G25SaveArtViewHistHandle(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
        _g25Repository = _unitOfWork.G25Repository;
    }

    public async Task<G25SaveArtViewHistResponse> ExecuteAsync(
        G25SaveArtViewHistRequest request,
        CancellationToken ct
    )
    {
        try
        {
            // Check if provided chapter detail is found or not.
            var chapterRepository = _unitOfWork.ChapterRepository;

            var isArtworkChapterExisted = await chapterRepository.IsChapterAvailableToDisplayByIdAsync(
                request.ArtworkId,
                request.ChapterId,
                request.UserId);

            if (!isArtworkChapterExisted)
            {
                return G25SaveArtViewHistResponse.FAILED;
            }

            var isViewHistoryRecordExisted = await _g25Repository.IsUserViewHistoryRecordExistedAsync(
                request.UserId,
                request.ArtworkId,
                ct);

            if (isViewHistoryRecordExisted)
            {
                return await UpdateViewHistoryRecordAsync(request, ct);
            }

            return await AddViewHistoryRecordAsync(request, ct);
        }
        catch
        {
            return G25SaveArtViewHistResponse.FAILED;
        }
    }

    private async Task<G25SaveArtViewHistResponse> UpdateViewHistoryRecordAsync(
        G25SaveArtViewHistRequest request, CancellationToken cancellationToken)
    {
        var viewedAt = DateTime.UtcNow;

        var updateResult = await _g25Repository.UpdateUserViewHistoryAsync(
            request.UserId,
            request.ArtworkId,
            request.ChapterId,
            cancellationToken);

        if (updateResult)
        {
            return G25SaveArtViewHistResponse.SUCCESS;
        }

        return G25SaveArtViewHistResponse.FAILED;
    }

    private async Task<G25SaveArtViewHistResponse> AddViewHistoryRecordAsync(
        G25SaveArtViewHistRequest request, CancellationToken cancellationToken)
    {
        var artworkType = await _g25Repository.GetArtworkTypeForAddingViewHistoryRecordAsync(
            request.ArtworkId,
            cancellationToken);

        var addResult = await _g25Repository.AddUserViewHistoryAsync(
            request.UserId,
            request.ArtworkId,
            request.ChapterId,
            artworkType,
            cancellationToken);

        if (addResult)
        {
            return G25SaveArtViewHistResponse.SUCCESS;
        }

        return G25SaveArtViewHistResponse.FAILED;
    }
}
