using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.G25.OtherHandlers.NumberArtViewed
{
    public class G25CountArtViewedHandler : IFeatureHandler<G25CountArtViewedRequest, G25CountArtViewedResponse>
    {
        private readonly IG25Repository _g25Repository;

        public G25CountArtViewedHandler(
            Lazy<IUnitOfWork> unitOfWork
        )
        {
            _g25Repository = unitOfWork.Value.G25Repository;
        }

        public async Task<G25CountArtViewedResponse> ExecuteAsync(G25CountArtViewedRequest request, CancellationToken ct)
        {
            if (request.UserId == 0)
            {
                return new G25CountArtViewedResponse { StatusCode = G25CountArtViewedResponseStatusCode.EMPTY };
            }

            return new G25CountArtViewedResponse
            {
                ArtCount = await _g25Repository.ArtworkHistoriesCount(request.UserId, request.ArtworkType, ct),
                StatusCode = G25CountArtViewedResponseStatusCode.SUCCESS,
            };
        }
    }
}
