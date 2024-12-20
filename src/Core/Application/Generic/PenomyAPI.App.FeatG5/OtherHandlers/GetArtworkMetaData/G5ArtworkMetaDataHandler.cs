using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG5.OtherHandlers.GetArtworkMetaData;

public class G5ArtworkMetaDataHandler : IFeatureHandler<G5ArtworkMetaDataRequest, G5ArtworkMetaDataResponse>
{
    private IG5Repository _g5Repository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public G5ArtworkMetaDataHandler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<G5ArtworkMetaDataResponse> ExecuteAsync(G5ArtworkMetaDataRequest request, CancellationToken ct)
    {
        var unitOfWork = _unitOfWork.Value;

        long GUEST_ID = -1;

        var isArtworkAvailable = await unitOfWork.ArtworkRepository.IsArtworkAvailableToDisplayByIdAsync(
            request.ArtworkId,
            GUEST_ID,
            ct);

        if (!isArtworkAvailable)
        {
            return G5ArtworkMetaDataResponse.ARTWORK_IS_NOT_FOUND;
        }

        _g5Repository = unitOfWork.FeatG5Repository;

        var metadata = await _g5Repository.GetArtworkMetaDataByIdAsync(request.ArtworkId, ct);

        return G5ArtworkMetaDataResponse.SUCCESS(metadata);
    }
}
