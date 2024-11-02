using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt12.OtherHandlers.ReloadChapterMedias;

public sealed class Art12ReloadChapterMediaHandler
    : IFeatureHandler<Art12ReloadChapterMediaRequest, Art12ReloadChapterMediaResponse>
{
    private readonly IArt12Repository _repository;

    public Art12ReloadChapterMediaHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.Art12Repository;
    }

    public async Task<Art12ReloadChapterMediaResponse> ExecuteAsync(Art12ReloadChapterMediaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var isChapterExisted = await _repository.IsComicChapterExistedAsync(
                request.ComicId,
                request.ChapterId,
                cancellationToken);

            if (!isChapterExisted)
            {
                return Art12ReloadChapterMediaResponse.CHAPTER_IS_NOT_FOUND;
            }

            var chapterMedias = await _repository.GetChapterMediasByChapterIdAsync(
                request.ChapterId,
                cancellationToken);

            return Art12ReloadChapterMediaResponse.SUCCESS(chapterMedias);
        }
        catch
        {
            return Art12ReloadChapterMediaResponse.DATABASE_ERROR;
        }
    }
}
