using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt1;

public sealed class Art1Handler : IFeatureHandler<Art1Request, Art1Response>
{
    private readonly IArt1Repository _art1Repository;

    public Art1Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _art1Repository = unitOfWork.Value.Art1Repository;
    }

    public async Task<Art1Response> ExecuteAsync(Art1Request request, CancellationToken ct)
    {
        var artworks = await _art1Repository.GetArtworksByTypeAndCreatorIdWithPaginationAsync(
            artworkType: request.ArtworkType,
            creatorId: request.CreatorId,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: ct);

        return new Art1Response
        {
            Artworks = artworks,
            AppCode = Art1ResponseAppCode.SUCCESS
        };
    }
}
