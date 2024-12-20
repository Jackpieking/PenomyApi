using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;

public sealed class Art12GetChapterDetailHandler
    : IFeatureHandler<Art12GetChapterDetailRequest, Art12GetChapterDetailResponse>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public Art12GetChapterDetailHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art12GetChapterDetailResponse> ExecuteAsync(
        Art12GetChapterDetailRequest request,
        CancellationToken cancellationToken)
    {
        var unitOfWork = _unitOfWork.Value;

        try
        {
            // Check if the input chapterId is existed or not.
            var isChapterExisted = await unitOfWork.ChapterRepository.IsChapterExistedByIdAsync(
                request.ChapterId,
                cancellationToken);

            if (!isChapterExisted)
            {
                return Art12GetChapterDetailResponse.CHAPTER_IS_NOT_FOUND;
            }

            // Check if the input chapterId is temporarily removed or not.
            var isTemporarilyRemoved = await unitOfWork.Art12Repository.IsChapterTemporarilyRemovedByIdAsync(
                request.ChapterId,
                cancellationToken);

            if (isTemporarilyRemoved)
            {
                return Art12GetChapterDetailResponse.CHAPTER_IS_TEMPORARILY_REMOVED;
            }

            // Check if the current creator has persmission
            // to access this chapter detail from the creator studio.
            var creatorHasPermissionToAccess = await unitOfWork.Art12Repository.HasPermissionToUpdateChapterDetailAsync(
                creatorId: request.CreatorId,
                chapterId: request.ChapterId,
                cancellationToken: cancellationToken);

            if (!creatorHasPermissionToAccess)
            {
                return Art12GetChapterDetailResponse.NO_PERMISSION_GRANTED_FOR_CURRENT_CREATOR;
            }

            // Get the detail of the chapter.
            var chapterDetail = await unitOfWork.Art12Repository.GetChapterDetailByIdAsync(
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

            return Art12GetChapterDetailResponse.SUCCESS(chapterDetail);
        }
        catch
        {
            return Art12GetChapterDetailResponse.DATABASE_ERROR;
        }
    }
}
