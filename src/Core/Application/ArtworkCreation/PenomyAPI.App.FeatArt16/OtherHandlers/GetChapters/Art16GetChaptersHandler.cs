using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt16.OtherHandlers.GetChapters;

public class Art16GetChaptersHandler
    : IFeatureHandler<Art16GetChaptersRequest, Art16GetChaptersResponse>
{
    private readonly IArt16Repository _repository;

    public Art16GetChaptersHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.Art16Repository;
    }

    public async Task<Art16GetChaptersResponse> ExecuteAsync(Art16GetChaptersRequest request, CancellationToken ct)
    {
        var chapters = await _repository.GetChaptersByPublishStatusAsync(
            request.AnimeId,
            request.PublishStatus,
            ct
        );

        return new() { Chapters = chapters, };
    }
}
