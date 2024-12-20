using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.APP.FeatG6;

public class G6Handler : IFeatureHandler<G6Request, G6Response>
{
    private const int DEFAULT_TOTAL_RECOMMENDED_ARTWORKS = 3;
    private const long DEFAULT_GUEST_ID = -1;
    private IG6Repository _g6Repository;
    private readonly IUnitOfWork _unitOfWork;

    public G6Handler(Lazy<IUnitOfWork> unitOfWork)
    {
        _unitOfWork = unitOfWork.Value;
    }

    public async Task<G6Response> ExecuteAsync(G6Request request, CancellationToken ct)
    {
        try
        {
            bool isArtworkAvailableToDisplayForGuest =
                await _unitOfWork.ArtworkRepository.IsArtworkAvailableToDisplayByIdAsync(
                    request.ArtworkId,
                    DEFAULT_GUEST_ID,
                    ct
                );

            if (!isArtworkAvailableToDisplayForGuest)
            {
                return G6Response.ARTWORK_ID_NOT_FOUND;
            }

            // Get the recommended artwork list for the current artwork.
            _g6Repository = _unitOfWork.G6Repository;

            if (request.TotalRecommendedArtworks < DEFAULT_TOTAL_RECOMMENDED_ARTWORKS)
            {
                request.TotalRecommendedArtworks = DEFAULT_TOTAL_RECOMMENDED_ARTWORKS;
            }

            var recommendedArtworkList = await _g6Repository.GetRecommendedArtworksAsync(
                request.ArtworkId,
                request.TotalRecommendedArtworks,
                ct
            );

            return G6Response.SUCCESS(recommendedArtworkList);
        }
        catch
        {
            return G6Response.DATABASE_ERROR;
        }
    }
}
