using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatArt6;

public sealed class Art6Handler : IFeatureHandler<Art6Request, Art6Response>
{
    private readonly IArt6Repository _repository;

    public Art6Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _repository = unitOfWork.Value.Art6Repository;
    }

    public async Task<Art6Response> ExecuteAsync(Art6Request request, CancellationToken ct)
    {
        var chapters = await _repository.GetChaptersByPublishStatusAsync(
            request.ComicId,
            request.PublishStatus,
            ct
        );

        return new() { Chapters = chapters, };
    }
}
