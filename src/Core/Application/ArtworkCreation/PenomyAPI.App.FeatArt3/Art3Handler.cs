using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatArt3;

public class Art3Handler : IFeatureHandler<Art3Request, Art3Response>
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private IArt3Repository _art3Repository;

    public Art3Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Art3Response> ExecuteAsync(Art3Request request, CancellationToken ct)
    {
        _art3Repository = _unitOfWork.Value.Art3Repository;

        var hasAnyDeletedItems = await _art3Repository.HasAnyDeletedItemsWithArtworTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        if (!hasAnyDeletedItems)
        {
            return Art3Response.Empty;
        }

        var deletedItems = await _art3Repository.GetAllDeletedArtworksByTypeAsync(
            request.CreatorId,
            request.ArtworkType,
            ct);

        return new()
        {
            IsEmpty = false,
            DeletedItems = deletedItems,
        };
    }
}
