using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;

public class Art22GetChapterDetailHandler
    : IFeatureHandler<Art22GetChapterDetailRequest, Art22GetChapterDetailResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt22Repository _art22Repository;

    public Art22GetChapterDetailHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art22GetChapterDetailResponse> ExecuteAsync(
        Art22GetChapterDetailRequest request,
        CancellationToken cancellationToken)
    {
        var unitOfWork = _unitOfWork.Value;

        try
        {
            // Check if the input chapterId is temporarily removed or not.
            _art22Repository = unitOfWork.Art22Repository;

            var isTemporarilyRemoved = await _art22Repository.IsChapterTemporarilyRemovedByIdAsync(
                request.ChapterId,
                cancellationToken);

            if (isTemporarilyRemoved)
            {
                return Art22GetChapterDetailResponse.CHAPTER_IS_TEMPORARILY_REMOVED;
            }

            // Check if the current creator has persmission
            // to access this chapter detail from the creator studio.
            var creatorHasPermissionToAccess = await _art22Repository.HasPermissionToUpdateChapterDetailAsync(
                creatorId: request.CreatorId,
                chapterId: request.ChapterId,
                cancellationToken: cancellationToken);

            if (!creatorHasPermissionToAccess)
            {
                return Art22GetChapterDetailResponse.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
            }

            // Get the detail of the chapter.
            var chapterDetail = await _art22Repository.GetChapterDetailByIdAsync(
                request.ChapterId,
                cancellationToken);

            // Sort items based on the upload order before displaying to the client.
            chapterDetail.ChapterMedias = chapterDetail.ChapterMedias.OrderBy(media => media.UploadOrder);

            // If the chapter is in drafted mode, then get the latest chapter upload order to display.
            if (chapterDetail.PublishStatus == PublishStatus.Drafted)
            {
                var lastChapterUploadOrder = await unitOfWork.ArtworkRepository.GetLastChapterUploadOrderByArtworkIdAsync(
                    chapterDetail.ArtworkId,
                    cancellationToken);

                chapterDetail.UploadOrder = lastChapterUploadOrder + 1;
            }

            return Art22GetChapterDetailResponse.SUCCESS(chapterDetail);
        }
        catch
        {
            return Art22GetChapterDetailResponse.DATABASE_ERROR;
        }
    }
}
